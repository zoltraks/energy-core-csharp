# Add configurable back-reference window to ZX0 compressor

**Type:** Feature

## Summary

Add a `ZX0.Options` class and `Compress` overloads so a caller can limit the maximum back-reference offset, mirroring the existing `LZSS.Options` pattern. The default window (32640 bytes) is preserved so existing callers see no change. Decompression is unaffected because the decoder reads offsets from the stream and needs no window parameter.

## Description

The ZX0 optimal parser already accepts an `offsetLimit` parameter internally. Every private method that the parser touches (`OffsetCeiling`, `InitialArenaCapacity`, `Optimizer.Optimize`) threads it through. The only place the full 32640-byte limit is wired in is the public `Compress(byte[] data)` method, which hardcodes `offsetLimit = MAX_OFFSET_ZX0`.

This change exposes that parameter through a public `Options` class on `Energy.Base.Compression.ZX0`, with a single field `MaxOffset`. A new `Compress(byte[] data, Options options)` overload passes `options.MaxOffset` into the optimizer. The existing parameterless `Compress(byte[] data)` keeps using the full 32640-byte window, so all current callers and all existing reference-fixture tests produce byte-identical output.

A window-limited stream is still a valid ZX0 stream. Any ZX0 decompressor can decode it. The constraint is one-directional: a stream compressed with a smaller window can be decompressed by any decoder, but a stream compressed with the full window may contain offsets that a constrained decoder (such as a 1024-byte ring-buffer player) cannot resolve. The caller is responsible for choosing a window that matches the target decoder.

`Decompress` does not gain an `Options` overload. The ZX0 bitstream is self-describing: offsets and lengths are encoded in the stream, and the decoder applies them regardless of the window that was used during compression. There is no decoder-side window to configure.

### Options class

A nested public class `ZX0.Options` with one field:

- `MaxOffset` (`int`, default 32640): the maximum back-reference distance in bytes the optimizer may use. Must be at least 1. Values above 32640 are clamped to 32640, which is the ZX0 format maximum.

The default constructor sets `MaxOffset = 32640`, matching the existing constant `MAX_OFFSET_ZX0`.

### Public API after the change

```csharp
// Existing, unchanged
byte[] Energy.Base.Compression.ZX0.Compress(byte[] data)
byte[] Energy.Base.Compression.ZX0.Decompress(byte[] data)

// New
byte[] Energy.Base.Compression.ZX0.Compress(byte[] data, Energy.Base.Compression.ZX0.Options options)
```

### Edge cases

- A `null` options reference is treated as the default (full window), so `Compress(data, null)` behaves like `Compress(data)`.
- A `MaxOffset` of 0 or negative is clamped to 1 (the minimum valid offset, matching `INITIAL_OFFSET`).
- A `MaxOffset` above 32640 is clamped to 32640.
- `null` or empty input handling is unchanged.

## Compatibility

The change adds one public overload and one public nested class. No existing method signature changes, so binary and source compatibility are preserved for all current callers.

The implementation uses only integer arithmetic and byte arrays. No LINQ, no `nameof`, no `string.Contains`, no auto-property initializers, no local functions, and no APIs unavailable on .NET 2.0 or the Compact Framework. The `Options` constructor assigns fields explicitly (no field initializers) for Compact Framework safety, matching the `LZSS.Options` pattern.

The optimizer remains per-call instance-based with no shared mutable static state, so `Compress` stays thread-safe.

## Use Cases

- When a caller compresses data for a decoder with a fixed-size ring buffer (for example a 1024-byte ring that can only resolve offsets up to 1023), the caller passes `MaxOffset = 1023` so no back-reference exceeds the decoder's window.
- When a caller compresses data for a full linear-buffer decoder (the default), the caller uses the parameterless overload or `new Options()` and gets the full 32640-byte window.
- When a caller passes `null` options, the result is identical to the parameterless overload.

## Hints

The internal plumbing already exists. The `Optimizer.Optimize` and `InitialArenaCapacity` methods already take `offsetLimit`. The change is to expose it publicly and add clamping logic. Study the `LZSS.Options` class and its `Compress(byte[], Options)` overload for the established pattern.

## Out of Scope

- Exposing `backwardsMode`, `classicMode`, or `invertMode` as options. These select different ZX0 format variants and are not needed for the window-limiting use case.
- Adding an `Options` overload to `Decompress`. The decoder does not need a window parameter.
- Changing the default window size. The default stays at 32640.
- Changing the `MAX_OFFSET_ZX0` constant or the existing fixture tests.
