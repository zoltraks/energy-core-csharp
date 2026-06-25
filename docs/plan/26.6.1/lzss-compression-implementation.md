# Add LZSS compression codec Implementation Plan

## Change Request Reference

This implementation plan is based on the change request at `docs/change/26.6.1/lzss-compression.md`.

## Best Practices

Follow the coding standards in GUIDELINES.md. The codec lives in library code, so it must compile on the Compact Framework and .NET 2.0: no LINQ, no `nameof`, no `string.Contains`, no auto-property initializers, no local functions, and fully qualified type references. All public methods carry XML documentation. The optimal parser keeps its state in per-call instances so `Compress` is thread-safe, mirroring the existing `ZX0` codec.

## Format Analysis

The reference per-stream LZSS, distilled from `work/lzss-sap/src/lzss.c`, is the contract this codec implements.

Token layout, read sequentially from the stream:

- A flag bit precedes every token. Flag bits are packed eight to a byte, least-significant bit first. A flag byte is emitted lazily the first time a token needs a flag bit and the current flag byte is full, and it physically sits immediately before that token's data bytes.
- Flag bit 1 means a literal: the next whole byte is the literal value.
- Flag bit 0 means a match. The match code encodes a position field of `offset bits` and a length field of `length bits`. The encoded length is `match length - minimum match length`. The encoded position is `(source index - base) AND (2^offset bits - 1)`, where base is 2 for the current format and 1 for the older format.

Match code byte layout depends on total bits:

- total <= 8: one byte, `(position << length bits) + length code`.
- total <= 12: one byte `(position << (8 - offset bits)) + (length code AND (2^(8 - offset bits) - 1))`, plus a half-byte `length code >> (8 - offset bits)`. Half-bytes pack two per byte, low nibble first, emitted lazily like flag bytes.
- total <= 16: two bytes, little endian, `((length code + 1) << offset bits) + position`.

Stream framing:

- When the first-byte-as-literal flag is set (current format), the stream begins with one raw byte (the first input byte), and token encoding covers positions 1 to size - 1. Otherwise the stream begins at position 0 with no raw byte.
- Force-last-literal: after the optimal parse, if the stream would end in a match, re-run the parse forcing the final byte to be a literal. This lets the reference players detect end of stream. The Energy.Core decoder terminates on input exhaustion and does not require it, but the encoder still applies it for byte-identical output.

Decoder offset recovery: at output position `pos` (equal to the current decoded length), with decoded position field `p` and base `C`, the back-reference distance is the unique value in `1 .. 2^offset bits` congruent to `pos - p - C` modulo `2^offset bits`. Matches may overlap, so the copy is byte by byte.

## Documentation Updates

- Update `project-energy/docs/source/base-compression.md` to add an `LZSS` section describing the codec, its parameters and defaults, the matching-parameters requirement on `Decompress`, and its relationship to the SAP-R reference. Place it after the existing codec sections and extend the "choosing a codec" guidance.
- No change to GUIDELINES.md or STRUCTURE.md: no rule or layout changes.

## Step by Step Implementation

**Add the LZSS options type**

A nested public class holding the configurable parameters with defaults set in a constructor (no field initializers on properties, for Compact Framework safety).

File to modify: `Energy.Core/Base/Compression.cs`.

    public class LZSS
    {
        public class Options
        {
            public int OffsetBits;        // default 4
            public int LengthBits;        // default 4
            public int MinimumMatch;      // default 2
            public bool ForceLastLiteral; // default true
            public bool LiteralFirst;     // default true
            public bool PositionStartZero;// default false
            public Options() { /* assign defaults */ }
        }
    }

**Add parameter validation and derived values**

A private helper validates ranges (offset 0..12, length >= 2, total 8..16, minimum 1..16) and computes `maxOffset = 1 << OffsetBits`, `maxLength = MinimumMatch + (1 << LengthBits) - 1`, and `base = PositionStartZero ? 1 : 2`. Invalid options cause `Compress`/`Decompress` to report through `Energy.Core.Bug.Catch` and return null, matching the other codecs' graceful failure style.

**Implement the optimal parser**

An instance-based parser that ports `match` and `lzop_backfill` from the reference, storing `bits`, `mlen`, and `mpos` arrays per call. No shared statics.

File to modify: `Energy.Core/Base/Compression.cs`.

    private sealed class Parser
    {
        // bits[], mlen[], mpos[] per position; Backfill(forceLastLiteral)
        // mirrors lzop_backfill cost model: bits_literal = 1 + 8,
        // bits_match = 1 + OffsetBits + LengthBits
    }

**Implement the bit/byte/half-byte writer**

A small writer that reproduces the reference `bf` buffer semantics exactly: a lazily allocated flag byte filled LSB-first, whole bytes appended at the growing end, and a lazily allocated half-byte holder filled low nibble then high nibble. This buffer management is load-bearing for byte-identical output.

    private sealed class StreamWriter
    {
        // AddBit, AddByte, AddHalfByte, ToArray
    }

**Implement the encoder**

Port `lzop_encode` over the parse: emit the optional first raw byte, then for each position emit a literal or a match code using the writer, honoring the total-bits branches.

**Implement the decoder**

A sequential reader with a flag-bit buffer and a half-byte buffer mirroring the writer. Read the optional first raw byte into the output, then loop while the input cursor has bytes left: read a flag bit, then a literal byte or a match code; decode the match into distance and length and copy byte by byte from the output, growing the output buffer with power-of-two growth as in the `ZX0` decoder.

**Add the public methods**

    public static byte[] Compress(byte[] data);
    public static byte[] Compress(byte[] data, Options options);
    public static byte[] Decompress(byte[] data);
    public static byte[] Decompress(byte[] data, Options options);

Each guards null and empty input, wraps the body in try/catch with `Energy.Core.Bug.Catch`, and returns null on failure.

## Implementation Order

- Update `project-energy` compression documentation.
- Add the `LZSS` class skeleton, `Options`, and validation in `Compression.cs`.
- Implement the parser, writer, encoder, and decoder.
- Add the public methods.
- Build the reference single-stream fixture generator under `work/`, generate fixtures, and copy them into `Energy.Core.Test/Resources/`.
- Add unit tests in `Energy.Core.Test/Base/Compression.cs`.
- Bump the version to 26.6.1 across both project files.
- Run the build and test verification loop.

## Testing Strategy

Add tests to `Energy.Core.Test/Base/Compression.cs` in the existing `Energy.Core.Test.Base` namespace, named so the production member is obvious (for example `Compression_LZSS_RoundTrip`, `Compression_LZSS_DefaultFixture`, `Compression_LZSS_NullAndEmpty`, `Compression_LZSS_Presets`).

- Null returns null; empty returns empty.
- Round trip for representative inputs across default, 12-bit, and 16-bit presets: `Decompress(Compress(x)) == x`.
- Reference compatibility, byte for byte: compare `Compress(x)` against committed fixtures produced by the reference algorithm, using `CollectionAssert.AreEqual`. Also assert `Decompress(referenceFixture) == x` to prove the decoder reads reference output.
- Edge inputs: single byte, highly repetitive data that exercises maximum-length matches and overlapping copies, and incompressible random data.

Fixtures are produced under `work/` by a single-stream build derived from the reference C sources (reusing the reference encoding functions with a minimal driver, since MSVC lacks the `getopt`/`unistd.h` the original CLI uses), then copied into `Energy.Core.Test/Resources/` and registered as embedded resources in `Energy.Core.Test.csproj`. Tests read only embedded resources, never files under `work/`.

## Verification

The implementation is complete only when all steps pass with zero errors and zero warnings.

- Build: `dotnet build Energy.Core/Energy.Core.csproj`
- Test: `dotnet test Energy.Core.Test/Energy.Core.Test.csproj`
- Confirm fixtures decode and re-encode identically for each supported preset.
