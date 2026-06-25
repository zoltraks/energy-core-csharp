# Add LZSS interleaved multi-channel mode

**Type:** Feature

## Summary

Extend the existing `Energy.Base.Compression.LZSS` codec so the same `Compress` and `Decompress` methods can also read and write the interleaved multi-channel container used by the Atari SAP-R compressor `dmsc/lzss-sap`. The mode is selected by a new option, leaving the default single-stream LZSS behaviour unchanged for generic use.

## Description

Today `LZSS.Compress` and `LZSS.Decompress` operate on one byte stream. The SAP-R format used by the reference tool is a container around that same per-stream LZSS: it splits a sequence of fixed-size frames into independent channels, compresses each channel with the LZSS scheme, and interleaves the channel tokens frame by frame behind a small header. The per-stream layer already lives in this codec; this change adds the container layer.

A new option, `FrameSize`, selects the mode. It is the number of bytes per frame, equal to the number of channels. When `FrameSize` is 0 or 1, the methods behave exactly as before: a single LZSS stream with no header. When `FrameSize` is 2 or more, the methods read and write the interleaved container.

The container layout, for `FrameSize` equal to N, is:

- A channel-skip header. One bit for each channel from N-1 down to 1, packed least-significant-bit first into whole bytes. Channel 0 is always present and has no bit. A set bit marks a channel whose value never changes across the whole input, so its tokens are omitted and only its initial value is stored.
- One initial byte for every channel, from channel N-1 down to channel 0. This is the value of that channel in the first frame.
- The interleaved token stream. For each frame position after the first, and for each non-skipped channel from N-1 down to 0, one LZSS literal-or-match token is emitted into a single shared bit stream. A channel covered by an in-progress match emits nothing for that frame.

Decoding mirrors the reference players. Each channel owns a ring buffer of `2^OffsetBits` entries with its initial value seeded at the last entry, and a write cursor that advances one position per frame and wraps around the ring. A match token names a start position and a copy count and copies bytes forward from the ring, so matches can reach the seeded initial value and can overlap. The decoded bytes are reassembled into frames in channel order 0 to N-1.

The token encoding itself, the optimal parse, the literal-first framing, the force-last-literal fixup, and the match-code bit layout are the same as the single-stream path and are shared. The reference applies the end-of-stream literal fixup to channel 0 only, and only when every active channel would otherwise end in a match; the container path follows that rule.

This is a pure addition to the public API surface: one new field on the existing `Options` type. No existing signature changes and no existing default changes.

## Compatibility

The change stays within the existing codec and uses only byte arrays and integer arithmetic, so it compiles on .NET Standard 2.0, .NET 4.0, .NET 3.5, .NET 2.0, and the Compact Framework with no conditional compilation. All working state is held in per-call locals and instances, so `Compress` and `Decompress` remain thread-safe.

## Use Cases

- When a caller decompresses a SAP-R `lz16` file with `FrameSize` 9, offset 8 bits, length 8 bits, and minimum match 1, the original 9-byte-per-frame POKEY register stream is recovered.
- When a caller compresses that same register stream with the same options, the output is byte-for-byte identical to the file the reference tool produces.
- When a caller leaves `FrameSize` at its default, `Compress` and `Decompress` behave exactly as before.

## Hints

The reference container logic is the `main` function in `work/lzss-sap/src/lzss.c`; the matching decoder is `work/lzss-sap/asm/playlzs16.asm`. The per-channel ring buffer with the seeded initial value and the shared flag-bit stream are the load-bearing details for byte-identical output.

## Out of Scope

- POKEY-specific input normalisation. The reference rewrites some AUDC register values to a canonical form while reading a raw SAP-R dump. The container here treats channel data as already canonical, so callers that need that normalisation must apply it before compressing. The values produced by decoding are already canonical, so a decode-then-encode round trip is unaffected.
- SAP file text headers, silence trimming, loop detection, and compression statistics.
- The older container format variant selected by the reference `-x` flag.
