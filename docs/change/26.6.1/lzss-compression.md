# Add LZSS compression codec

**Type:** Feature

## Summary

Add an `LZSS` codec to `Energy.Base.Compression` that implements the LZSS variant used by the Atari SAP-R compressor `dmsc/lzss-sap`. The codec works on an arbitrary single byte stream, is fully configurable, and is bit-for-bit compatible with the per-stream encoding of the reference tool so that data compressed by Energy.Core decodes with the reference players and data produced by the reference tool decodes with Energy.Core.

## Description

`Energy.Base.Compression.LZSS` exposes static `Compress` and `Decompress` methods with the same `byte[]` in, `byte[]` out shape as the existing `Deflate`, `GZip`, `ZX0`, and `LZ4` codecs. A `null` input returns `null`. An empty input returns an empty array.

The codec is a generic single-stream LZSS, not a SAP-R container. The reference tool reads a 9-register POKEY dump, splits it into nine independent streams, compresses each stream with this LZSS scheme, and interleaves them frame by frame with a small channel header. This change implements only the per-stream LZSS layer, which is the reusable, format-defining part. Building a SAP-R 9-channel container on top of this codec is a separate concern and is out of scope.

The encoder uses the same optimal backward parse as the reference, so for a given input and parameters it selects the same literal and match decisions and produces a byte-identical stream. The match and literal encoding, the bit and byte interleaving, and the half-byte packing for wider match codes all match the reference bit layout.

The format is parameterised, matching the reference tool's options. The default values match the tool's `-8` preset, which is the most common configuration and the one decoded by `asm/playlzs.asm`.

Default parameters:

- match offset bits: 4 (window of 16)
- match length bits: 4 (lengths 2..17)
- minimum match length: 2
- force a literal at end of stream: true
- store the first byte as a literal and start matching at position 1: true
- match position base: start at maximum, not zero

Configurable parameters allow other presets and custom widths:

- offset bits from 0 to 12
- length bits from 2 up, with total (offset + length) bits from 8 to 16
- minimum match length from 1 to 16
- the two format-version flags that select between the current format and the older format the reference exposes with `-x`

Because a raw LZSS stream carries no parameter header and no length prefix, `Decompress` must be called with the same parameters used to compress. The default `Decompress` overload assumes the default parameters. This matches the reference design, where each assembly player is hard-wired to one parameter set.

## Compatibility

The codec must compile on .NET Standard 2.0, .NET 4.0, .NET 3.5, .NET 2.0, and the Compact Framework. It uses only byte arrays and simple integer arithmetic, with no LINQ, no `string` APIs, no reflection, and no framework compression types, so no conditional compilation is expected. The optimal parser holds all working state in per-call instances with no shared mutable static state, so `Compress` is thread-safe, consistent with the existing `ZX0` codec.

## Use Cases

- When a caller compresses a byte stream with default parameters, the output is identical to the stream the reference tool produces for that data as a single active channel with its default options, and the reference players decode it.
- When a caller decompresses, with default parameters, a single LZSS stream produced by the reference tool, the original bytes are recovered.
- When a caller needs a wider window or longer matches, the offset, length, and minimum-match parameters select the 12-bit or 16-bit presets, or any custom width the reference supports.

## Hints

Study the existing `ZX0` codec in `Energy.Core/Base/Compression.cs` for the instance-based, allocation-light optimizer pattern and the per-call thread-safety approach. The reference algorithm lives in `work/lzss-sap/src/lzss.c`; the matching decoders are the `asm/playlzs*.asm` players.

## Out of Scope

- The SAP-R 9-channel container: channel-skip header, per-channel initial values, frame interleaving, silence normalisation, trimming, and loop detection.
- Reading or writing SAP file text headers.
- Compression statistics reporting.
