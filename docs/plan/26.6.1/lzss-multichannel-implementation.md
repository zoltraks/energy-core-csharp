# Add LZSS interleaved multi-channel mode Implementation Plan

## Change Request Reference

This implementation plan is based on the change request at `docs/change/26.6.1/lzss-multichannel.md`.

## Best Practices

Follow the coding standards in GUIDELINES.md. The codec lives in library code, so it must compile on the Compact Framework and .NET 2.0: no LINQ, no `nameof`, no `string.Contains`, no auto-property initializers, no local functions, and fully qualified type references. The new state stays in per-call locals so `Compress` and `Decompress` remain thread-safe, matching the existing path.

## Format Analysis

The container, distilled from `work/lzss-sap/src/lzss.c` and `work/lzss-sap/asm/playlzs16.asm`, wraps the existing per-stream LZSS.

Header and initial values, for `FrameSize` equal to N:

- Channel-skip header: for channel index from N-1 down to 1, add one flag bit, least-significant-bit first, set when that channel is constant across the whole input. Channel 0 is always active and gets no bit. The partial flag byte is then flushed.
- Initial values: one raw byte for each channel from N-1 down to 0, the first-frame value of that channel. The buffer is flushed again so the token stream starts on a fresh flag byte.

Token stream:

- For each frame position from 1 to frames-1, and for each non-skipped channel from N-1 down to 0, encode one token with the shared writer using the existing literal and match-code layout. A channel whose last match still covers this position emits nothing.
- The optimal parse runs per channel. The force-last-literal fixup is applied to channel 0 only, and only when every active channel would otherwise end in a match.

Decoder ring model, independent of the Atari memory layout:

- Frame k's value lives at ring index `(k-1) mod M`, where `M = 2^OffsetBits`. The initial value (frame 0) therefore sits at index `M-1`.
- A match names `codePos` and a copy count. With `base = PositionStartZero ? 1 : 2`, the first copied byte is ring index `(codePos + base - 1) mod M`, which equals the index of the referenced frame. Each further copied byte advances one index. The decoder writes each produced byte to the current cursor and to the channel output, so overlapping matches reproduce correctly.

This inverts the encoder's `codePos = (pos - matchPos - base) AND (M-1)` for any offset width and either base, so no Atari-specific addressing is reproduced.

## Documentation Updates

- Update `project-energy/docs/source/base-compression.md`: extend the `LZSS` section with the `FrameSize` option, the container layout, the matching-parameters requirement, and the SAP-R relationship.
- No GUIDELINES.md or STRUCTURE.md change: no rule or layout change.

## Step by Step Implementation

**Add the FrameSize option**

Add one field to `Options` with its default set in the constructor.

File to modify: `Energy.Core/Base/Compression.cs`.

    public int FrameSize;   // default 0 = single stream; >= 2 = interleaved container

**Extend validation**

`ValidateOptions` rejects `FrameSize` below 0. When `FrameSize` is 2 or more it also requires `LiteralFirst`, because the container always stores an initial byte per channel and the decoder relies on it.

**Share the token encoder**

Extract the per-token literal/match emission from the single-stream `Compress` loop into one private static helper so both paths emit identical bytes.

    private static int EncodeToken(Writer writer, int[] matchLength, int[] matchPos,
        byte[] data, int pos, int minMatch, int lengthBits, int offsetBits,
        int maxOffset, int positionBase, int total)

**Add a Flush to the writer**

Add `Writer.Flush` that abandons any partial flag byte and half-byte holder, mirroring the reference `bflush` boundary between the header, the initial values, and the token stream.

**Implement the container compressor**

A private static `CompressChannels` deinterleaves the input into N channel arrays, detects constant channels, writes the skip header and initial bytes, runs a per-channel parser with the channel-0 force-last-literal rule, then interleaves tokens through the shared writer and helper.

**Implement the container decompressor**

A private static `DecompressChannels` reads the skip header and initial bytes, seeds each channel ring at index `M-1`, then walks frames: for each active channel either continue an in-progress copy or read a new token, write the produced byte to the ring cursor and the channel output, and advance the cursor modulo M. It reassembles frames in channel order 0 to N-1.

**Route the public methods**

In `Compress` and `Decompress`, after validation, branch to the container helpers when `options.FrameSize >= 2`, otherwise run the existing single-stream body. The null and empty guards and the `Energy.Core.Bug.Catch` failure handling are unchanged.

## Implementation Order

- Update the project-energy compression documentation.
- Add the `FrameSize` option and validation.
- Add `Writer.Flush` and extract `EncodeToken`; confirm existing single-stream tests still pass.
- Implement `CompressChannels` and `DecompressChannels` and route the public methods.
- Build the reference-derived multi-channel fixture generator under `work/`, generate fixtures, and copy them with their inputs into `Energy.Core.Test/Resources/`.
- Add unit tests in `Energy.Core.Test/Base/Compression.cs`.
- Run the build and test verification loop.

## Testing Strategy

Add tests to `Energy.Core.Test/Base/Compression.cs` in the existing `Energy.Core.Test.Base` namespace, named so the production member is obvious, for example `Compression_LZSS_MultiChannelFixtures`, `Compression_LZSS_MultiChannelRoundTrip`, and `Compression_LZSS_MultiChannelInvalidOptions`.

- Reference compatibility, byte for byte: `Compress(input, options)` equals a committed reference container fixture, and `Decompress(fixture, options)` reproduces the input, using `CollectionAssert.AreEqual`.
- A genuine external SAP-R `lz16` file (a real POKEY register dump compressed by the upstream tool) is committed as the primary fixture; it exercises the 16-bit preset, ring wrap, literals, and matches with all channels active.
- A synthetic fixture produced by a reference-derived generator exercises the skip header, with one or more constant channels and a length above the ring size to cover a wrap.
- Round trip across representative channel data confirms `Decompress(Compress(x)) == x` in container mode.
- Error cases: `FrameSize` below 0 and `FrameSize` of 2 or more with `LiteralFirst` false both fail to null.
- Existing single-stream tests must continue to pass unchanged, proving the default path is untouched.

Fixtures are generated under `work/` by a driver that reuses the reference encoding functions and the reference container logic from `work/lzss-sap/src/lzss.c`, then copied into `Energy.Core.Test/Resources/` and registered as embedded resources. Tests read only embedded resources, never files under `work/`.

## Verification

The implementation is complete only when all steps pass with zero errors and zero warnings.

- Build: `dotnet build Energy.Core/Energy.Core.csproj`
- Test: `dotnet test Energy.Core.Test/Energy.Core.Test.csproj`
- Confirm the committed reference container fixtures both decode and re-encode identically, and that the external SAP-R file round-trips.
