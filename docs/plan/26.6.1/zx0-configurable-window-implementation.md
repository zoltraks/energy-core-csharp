# Add configurable back-reference window to ZX0 compressor Implementation Plan

## Change Request Reference

This implementation plan is based on the change request at `docs/change/26.6.1/zx0-configurable-window.md`.

## Best Practices

Follow the coding standards in GUIDELINES.md. The code lives in library code, so it must compile on the Compact Framework and .NET 2.0: no LINQ, no `nameof`, no `string.Contains`, no auto-property initializers, no local functions, and fully qualified type references. All public methods and types carry XML documentation. The optimizer keeps its state in per-call instances with no shared mutable static state, so `Compress` stays thread-safe.

## Documentation Updates

- Update `project-energy/docs/source/base-compression.md` to add an `Options` subsection under the existing ZX0 section, describing the `MaxOffset` field, its default, the clamping rules, and the new `Compress(byte[], Options)` overload. Place it after the existing ZX0 method signatures and before the LZ4 section.
- No change to GUIDELINES.md or STRUCTURE.md: no rule or layout changes.

## Step by Step Implementation

**Add the ZX0.Options class**

A nested public class with one field and a default constructor, mirroring the `LZSS.Options` pattern. Fields are assigned in the constructor (no field initializers) for Compact Framework safety.

File to modify: `Energy.Core/Base/Compression.cs`, inside the `ZX0` class before the existing `#region Compress`.

    public class Options
    {
        /// <summary>Maximum back-reference offset in bytes (1 to 32640)</summary>
        public int MaxOffset;

        /// <summary>
        /// Create options with the default full ZX0 window (32640 bytes).
        /// </summary>
        public Options()
        {
            MaxOffset = MAX_OFFSET_ZX0;
        }
    }

**Add the offset clamping helper**

A private static method that clamps the caller-supplied value into the valid range. Returns at least 1 (INITIAL_OFFSET) and at most MAX_OFFSET_ZX0.

File to modify: `Energy.Core/Base/Compression.cs`.

    private static int ClampMaxOffset(int maxOffset)
    {
        if (maxOffset < INITIAL_OFFSET)
            return INITIAL_OFFSET;
        if (maxOffset > MAX_OFFSET_ZX0)
            return MAX_OFFSET_ZX0;
        return maxOffset;
    }

**Add the Compress overload**

A new public method that accepts `Options`, resolves the effective `MaxOffset`, and passes it as `offsetLimit` into the existing optimizer pipeline. The body mirrors `Compress(byte[])` exactly except for the `offsetLimit` source and the null-options fallback.

File to modify: `Energy.Core/Base/Compression.cs`.

    /// <summary>
    /// Compress data using the ZX0 algorithm with the given options.
    /// <br/><br/>
    /// A null options reference is treated as the default full-window options.
    /// </summary>
    /// <param name="data">Uncompressed data to compress</param>
    /// <param name="options">Compression options, or null for defaults</param>
    /// <returns>Compressed ZX0 stream</returns>
    public static byte[] Compress(byte[] data, Options options)
    {
        int offsetLimit = (options == null)
            ? MAX_OFFSET_ZX0
            : ClampMaxOffset(options.MaxOffset);
        return CompressWithLimit(data, offsetLimit);
    }

**Refactor the existing Compress to share the core**

Extract the shared compression body into a private static method that takes the resolved `offsetLimit`, so both the parameterless and the options overload use the same code path. The parameterless overload calls it with `MAX_OFFSET_ZX0`.

File to modify: `Energy.Core/Base/Compression.cs`.

    private static byte[] CompressWithLimit(byte[] data, int offsetLimit)
    {
        try
        {
            if (data == null) return null;
            if (data.Length == 0) return new byte[0];

            int inputSize = data.Length;
            int skip = 0;

            Optimizer optimizer = new Optimizer(InitialArenaCapacity(inputSize, offsetLimit));
            int optimal = optimizer.Optimize(data, inputSize, skip, offsetLimit);
            if (optimal == -1)
                return new byte[0];

            bool backwardsMode = false;
            bool classicMode = false;
            bool invertMode = !classicMode && !backwardsMode;

            return optimizer.Encode(optimal, data, skip, backwardsMode, invertMode);
        }
        catch (Exception exception)
        {
            Energy.Core.Bug.Catch(exception);
            return null;
        }
    }

The existing `Compress(byte[])` becomes:

    public static byte[] Compress(byte[] data)
    {
        return CompressWithLimit(data, MAX_OFFSET_ZX0);
    }

**Add unit tests**

Add tests to `Energy.Core.Test/Base/Compression.cs` in the existing `Energy.Core.Test.Base` namespace.

File to modify: `Energy.Core.Test/Base/Compression.cs`.

Tests to add:

- `Compression_ZX0_Options_NullMatchesDefault`: `Compress(data, null)` produces the same bytes as `Compress(data)` for a representative input.
- `Compression_ZX0_Options_DefaultMatchesParameterless`: `Compress(data, new Options())` produces the same bytes as `Compress(data)`.
- `Compression_ZX0_Options_SmallWindowRoundTrip`: compress with `MaxOffset = 1023`, decompress with the parameterless `Decompress`, and verify the round trip reproduces the input.
- `Compression_ZX0_Options_SmallWindowLimitsOffsets`: construct an input that contains a long-range repeat at a distance above the small window (for example a 250-byte block repeated 750 bytes apart), compress with `MaxOffset = 1023` and with the default window, and assert the small-window output differs from the default-window output. This proves the window constraint is active.
- `Compression_ZX0_Options_Clamping`: `MaxOffset = 0` and `MaxOffset = -1` both behave like `MaxOffset = 1`; `MaxOffset = 99999` behaves like the default 32640.
- `Compression_ZX0_Options_NullAndEmpty`: `Compress(null, new Options())` returns null; `Compress(new byte[0], new Options())` returns empty.

## Implementation Order

- Update `project-energy/docs/source/base-compression.md` with the ZX0 Options subsection.
- Add the `Options` class, `ClampMaxOffset`, and the `Compress(byte[], Options)` overload to `Compression.cs`.
- Refactor `Compress(byte[])` to delegate to `CompressWithLimit`.
- Add unit tests to `Energy.Core.Test/Base/Compression.cs`.
- Run the build and test verification loop.

## Testing Strategy

The existing ZX0 fixture tests (`Compression_ZX0_ZX0500Fixture`, `Compression_ZX0_TestFile`, `Compression_ZX0_Big16KFixture`, `Compression_ZX0_Random8KFixture`, and others) must continue to pass unchanged, proving the default window behaviour is preserved.

The new tests cover:

- Null and empty input with options.
- Null and default options producing the same output as the parameterless overload.
- A small window (1023) producing a valid round-trip through the parameterless decoder.
- A small window producing different output than the default window on input with long-range repeats, proving the constraint is active.
- Clamping of out-of-range `MaxOffset` values.

No new fixtures are needed. The existing fixtures and round-trip checks are sufficient because a window-limited stream is still a valid ZX0 stream decodable by the existing `Decompress`.

## Verification

The implementation is complete only when all steps pass with zero errors and zero warnings.

- Build: `dotnet build Energy.Core/Energy.Core.csproj`
- Test: `dotnet test Energy.Core.Test/Energy.Core.Test.csproj`
- Confirm all existing ZX0 fixture tests still pass byte-for-byte.
- Confirm the new window-limit tests pass.
