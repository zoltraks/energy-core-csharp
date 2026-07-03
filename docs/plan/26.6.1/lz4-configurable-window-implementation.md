# Add configurable back-reference window to LZ4 compressor Implementation Plan

## Change Request Reference

This implementation plan is based on the change request at `docs/change/26.6.1/lz4-configurable-window.md`.

## Best Practices

Follow the coding standards in GUIDELINES.md. The code lives in library code, so it must compile on the Compact Framework and .NET 2.0: no LINQ, no `nameof`, no `string.Contains`, no auto-property initializers, no local functions, and fully qualified type references. All public methods and types carry XML documentation. The compressor allocates its hash table per call, so `Compress` stays thread-safe.

## Documentation Updates

- Update `project-energy/docs/source/base-compression.md` to add an `Options` subsection under the existing LZ4 section, describing the `MaxOffset` field, its default, the clamping rules, and the new `Compress(byte[], Options)` overload. Place it after the existing LZ4 method signatures and example.
- No change to GUIDELINES.md or STRUCTURE.md: no rule or layout changes.

## Step by Step Implementation

**Add the LZ4.Options class**

A nested public class with one field and a default constructor, mirroring the `ZX0.Options` and `LZSS.Options` patterns. Fields are assigned in the constructor (no field initializers) for Compact Framework safety.

File to modify: `Energy.Core/Base/Compression.cs`, inside the `LZ4` class before the existing `#region Compress`.

    public class Options
    {
        /// <summary>Maximum back-reference offset in bytes (1 to 65535).</summary>
        public int MaxOffset;

        /// <summary>
        /// Create options with the default full LZ4 window (65535 bytes).
        /// </summary>
        public Options()
        {
            MaxOffset = MAX_OFFSET;
        }
    }

**Add the offset clamping helper**

A private static method that clamps the caller-supplied value into the valid range. Returns at least 1 and at most MAX_OFFSET (65535).

File to modify: `Energy.Core/Base/Compression.cs`.

    private static int ClampMaxOffset(int maxOffset)
    {
        if (maxOffset < 1)
            return 1;
        if (maxOffset > MAX_OFFSET)
            return MAX_OFFSET;
        return maxOffset;
    }

**Refactor the existing Compress to accept an offset limit**

Extract the shared compression body into a private static method that takes the resolved `maxOffset`, so both the parameterless and the options overload use the same code path. The only change inside the body is replacing the hardcoded `MAX_OFFSET` in the match search with the `maxOffset` parameter.

File to modify: `Energy.Core/Base/Compression.cs`.

The existing `Compress(byte[])` becomes:

    public static byte[] Compress(byte[] data)
    {
        return CompressWithLimit(data, MAX_OFFSET);
    }

The new overload:

    /// <summary>
    /// Compress data using the LZ4 block format with the given options.
    /// <br/><br/>
    /// A null options reference is treated as the default full-window options.
    /// </summary>
    /// <param name="data">Uncompressed input data</param>
    /// <param name="options">Compression options, or null for defaults</param>
    /// <returns>Compressed LZ4 block</returns>
    public static byte[] Compress(byte[] data, Options options)
    {
        int maxOffset = (options == null)
            ? MAX_OFFSET
            : ClampMaxOffset(options.MaxOffset);
        return CompressWithLimit(data, maxOffset);
    }

The private core method:

    private static byte[] CompressWithLimit(byte[] data, int maxOffset)
    {
        if (data == null)
            return null;
        if (data.Length == 0)
            return new byte[0];
        if (data.Length < 13)
            return CompressLiteralsOnly(data);

        try
        {
            // ... same body as current Compress, but replace
            //   (ip - refPos) <= MAX_OFFSET
            // with
            //   (ip - refPos) <= maxOffset
        }
        catch (Exception exception)
        {
            Energy.Core.Bug.Catch(exception);
            return null;
        }
    }

**Add unit tests**

Add tests to `Energy.Core.Test/Base/Compression.cs` in the existing `Energy.Core.Test.Base` namespace, inside the `#region LZ4` section.

File to modify: `Energy.Core.Test/Base/Compression.cs`.

Tests to add:

- `Compression_LZ4_Options_NullMatchesDefault`: `Compress(data, null)` produces the same bytes as `Compress(data)` for a representative input.
- `Compression_LZ4_Options_DefaultMatchesParameterless`: `Compress(data, new Options())` produces the same bytes as `Compress(data)`.
- `Compression_LZ4_Options_SmallWindowRoundTrip`: compress with `MaxOffset = 1023`, decompress with the parameterless `Decompress`, and verify the round trip reproduces the input.
- `Compression_LZ4_Options_SmallWindowLimitsOffsets`: construct an input with a long-range repeat at a distance above the small window (a 250-byte block repeated 800 bytes apart, total distance 1050), compress with `MaxOffset = 1023` and with the default window, and assert the small-window output differs from the default-window output. Both must still decode back to the original.
- `Compression_LZ4_Options_Clamping`: `MaxOffset = 0` and `MaxOffset = -1` both behave like `MaxOffset = 1`; `MaxOffset = 99999` behaves like the default 65535.
- `Compression_LZ4_Options_NullAndEmpty`: `Compress(null, new Options())` returns null; `Compress(new byte[0], new Options())` returns empty.

## Implementation Order

- Update `project-energy/docs/source/base-compression.md` with the LZ4 Options subsection.
- Add the `Options` class, `ClampMaxOffset`, and the `Compress(byte[], Options)` overload to `Compression.cs`.
- Refactor `Compress(byte[])` to delegate to `CompressWithLimit`.
- Add unit tests to `Energy.Core.Test/Base/Compression.cs`.
- Run the build and test verification loop.

## Testing Strategy

The existing LZ4 fixture tests (`Compression_LZ4_DecompressSmallFixture`, `Compression_LZ4_DecompressPatternFixture`, `Compression_LZ4_RoundTripSmall`, `Compression_LZ4_RoundTripPattern`, `Compression_LZ4_RoundTripRandom`, `Compression_LZ4_RoundTripLarge`, `Compression_LZ4_NullData`) must continue to pass unchanged, proving the default window behaviour is preserved.

The new tests cover:

- Null and empty input with options.
- Null and default options producing the same output as the parameterless overload.
- A small window (1023) producing a valid round-trip through the parameterless decoder.
- A small window producing different output than the default window on input with long-range repeats, proving the constraint is active.
- Clamping of out-of-range `MaxOffset` values.

No new fixtures are needed. The existing fixtures and round-trip checks are sufficient because a window-limited stream is still a valid LZ4 block decodable by the existing `Decompress`.

## Verification

The implementation is complete only when all steps pass with zero errors and zero warnings.

- Build: `dotnet build Energy.Core/Energy.Core.csproj`
- Test: `dotnet test Energy.Core.Test/Energy.Core.Test.csproj`
- Confirm all existing LZ4 fixture tests still pass.
- Confirm the new window-limit tests pass.
