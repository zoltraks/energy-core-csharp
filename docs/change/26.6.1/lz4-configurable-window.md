# Add configurable back-reference window to LZ4 compressor

**Type:** Feature

## Summary

Add an `LZ4.Options` class and `Compress` overloads so a caller can limit the maximum back-reference offset, mirroring the `ZX0.Options` and `LZSS.Options` patterns. The default window (65535 bytes) is preserved so existing callers see no change. Decompression is unaffected because the decoder reads offsets from the stream and needs no window parameter.

## Description

The LZ4 compressor currently hardcodes `MAX_OFFSET = 65535` as a private constant and uses it directly in the hash-table match search. Unlike ZX0 (whose optimizer already accepted an `offsetLimit` parameter internally) and LZSS (which has a full `Options` class), LZ4 has no public way to constrain the back-reference window.

This change exposes the offset limit through a public `Options` class on `Energy.Base.Compression.LZ4`, with a single field `MaxOffset`. A new `Compress(byte[] data, Options options)` overload passes `options.MaxOffset` into the compressor. The existing parameterless `Compress(byte[] data)` keeps using the full 65535-byte window, so all current callers and all existing fixture tests produce byte-identical output.

A window-limited stream is still a valid LZ4 raw block. Any LZ4 decompressor can decode it. The constraint is one-directional: a stream compressed with a smaller window can be decompressed by any decoder, but a stream compressed with the full window may contain offsets that a constrained decoder (such as a 1024-byte ring-buffer player) cannot resolve. The caller is responsible for choosing a window that matches the target decoder.

`Decompress` does not gain an `Options` overload. The LZ4 block format is self-describing: offsets and lengths are encoded in the stream, and the decoder applies them regardless of the window that was used during compression. There is no decoder-side window to configure.

### Options class

A nested public class `LZ4.Options` with one field:

- `MaxOffset` (`int`, default 65535): the maximum back-reference distance in bytes the compressor may use. Must be at least 1. Values above 65535 are clamped to 65535, which is the LZ4 16-bit offset maximum.

The default constructor sets `MaxOffset = 65535`, matching the existing constant `MAX_OFFSET`.

### Public API after the change

```csharp
// Existing, unchanged
byte[] Energy.Base.Compression.LZ4.Compress(byte[] data)
byte[] Energy.Base.Compression.LZ4.Decompress(byte[] data)

// New
byte[] Energy.Base.Compression.LZ4.Compress(byte[] data, Energy.Base.Compression.LZ4.Options options)
```

### Edge cases

- A `null` options reference is treated as the default (full window), so `Compress(data, null)` behaves like `Compress(data)`.
- A `MaxOffset` of 0 or negative is clamped to 1 (the minimum valid offset).
- A `MaxOffset` above 65535 is clamped to 65535.
- `null` or empty input handling is unchanged.

## Compatibility

The change adds one public overload and one public nested class. No existing method signature changes, so binary and source compatibility are preserved for all current callers.

The implementation uses only integer arithmetic, byte arrays, and `List<byte>`. No LINQ, no `nameof`, no `string.Contains`, no auto-property initializers, no local functions, and no APIs unavailable on .NET 2.0 or the Compact Framework. The `Options` constructor assigns fields explicitly (no field initializers) for Compact Framework safety, matching the `LZSS.Options` and `ZX0.Options` patterns.

The compressor remains stateless across calls (the hash table is allocated per call), so `Compress` stays thread-safe.

## Use Cases

- When a caller compresses data for a decoder with a fixed-size ring buffer (for example a 1024-byte ring that can only resolve offsets up to 1023), the caller passes `MaxOffset = 1023` so no back-reference exceeds the decoder's window.
- When a caller compresses data for a full linear-buffer decoder (the default), the caller uses the parameterless overload or `new Options()` and gets the full 65535-byte window.
- When a caller passes `null` options, the result is identical to the parameterless overload.

## Hints

The match search at the line `if (refPos >= 0 && refPos < ip && (ip - refPos) <= MAX_OFFSET)` is the only place the offset limit is enforced. Replace the constant with a local variable initialized from the resolved options. Study the `ZX0.Options` class and its `Compress(byte[], Options)` overload for the established pattern.

## Out of Scope

- Adding a size prefix or header to the LZ4 block output. The compressor still produces a raw LZ4 block with no framing.
- Adding an `Options` overload to `Decompress`. The decoder does not need a window parameter.
- Changing the default window size. The default stays at 65535.
- Changing the `MAX_OFFSET` constant or the existing fixture tests.
- Changing the hash table size or the greedy match strategy. Only the offset limit is parameterized.
