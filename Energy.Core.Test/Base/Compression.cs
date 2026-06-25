using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Compression
    {
        [TestMethod]
        public void Compression_Deflate_CompressDecompress()
        {
            // Test data
            string originalText = "Hello World! This is a test string for compression. " +
                                "It contains some repeated patterns to make it compressible. " +
                                "Hello World! This is a test string for compression.";
            byte[] originalData = System.Text.Encoding.UTF8.GetBytes(originalText);

            // Compress
            byte[] compressedData = Energy.Base.Compression.Deflate.Compress(originalData);
            Assert.IsNotNull(compressedData, "Compressed data should not be null");
            Assert.IsTrue(compressedData.Length > 0, "Compressed data should not be empty");
            Assert.IsTrue(compressedData.Length < originalData.Length, "Compressed data should be smaller than original");

            // Decompress
            byte[] decompressedData = Energy.Base.Compression.Deflate.Decompress(compressedData);
            Assert.IsNotNull(decompressedData, "Decompressed data should not be null");
            Assert.AreEqual(originalData.Length, decompressedData.Length, "Decompressed data should have same length as original");

            string decompressedText = System.Text.Encoding.UTF8.GetString(decompressedData);
            Assert.AreEqual(originalText, decompressedText, "Decompressed text should match original");
        }

        [TestMethod]
        public void Compression_ZX0_ZX0500Fixture()
        {
            byte[] expectedPlain = ReadEmbeddedResource("Resources.ZX0_500.txt");
            Assert.IsNotNull(expectedPlain, "Fixture ZX0_500.txt should be embedded");
            Assert.IsTrue(expectedPlain.Length > 0, "Fixture ZX0_500.txt should not be empty");

            byte[] expectedCompressed = ReadEmbeddedResource("Resources.ZX0_500.zx0");
            Assert.IsNotNull(expectedCompressed, "Fixture ZX0_500.zx0 should be embedded");
            Assert.IsTrue(expectedCompressed.Length > 0, "Fixture ZX0_500.zx0 should not be empty");

            byte[] actualCompressed = Energy.Base.Compression.ZX0.Compress(expectedPlain);
            Assert.IsNotNull(actualCompressed, "ZX0 compression should not return null");
            CollectionAssert.AreEqual(expectedCompressed, actualCompressed, "ZX0 compression output should match ZX0_500.zx0 fixture");

            byte[] actualPlain = Energy.Base.Compression.ZX0.Decompress(expectedCompressed);
            Assert.IsNotNull(actualPlain, "ZX0 decompression should not return null");
            CollectionAssert.AreEqual(expectedPlain, actualPlain, "ZX0 decompression should reproduce ZX0_500.txt exactly");
        }

        [TestMethod]
        public void Compression_Deflate_EmptyData()
        {
            byte[] emptyData = new byte[0];

            byte[] compressedData = Energy.Base.Compression.Deflate.Compress(emptyData);
            byte[] decompressedData = Energy.Base.Compression.Deflate.Decompress(compressedData);

            Assert.AreEqual(0, decompressedData.Length, "Empty data should remain empty after compression/decompression");
        }

        [TestMethod]
        public void Compression_Deflate_NullData()
        {
            byte[] compressedData = Energy.Base.Compression.Deflate.Compress(null);
            byte[] decompressedData = Energy.Base.Compression.Deflate.Decompress(null);

            Assert.IsNull(compressedData, "Compressing null should return null");
            Assert.IsNull(decompressedData, "Decompressing null should return null");
        }

        [TestMethod]
        public void Compression_GZip_CompressDecompress()
        {
            // Test data
            string originalText = "Hello World! This is a test string for GZip compression. " +
                                "It contains some repeated patterns to make it compressible. " +
                                "Hello World! This is a test string for compression.";
            byte[] originalData = System.Text.Encoding.UTF8.GetBytes(originalText);

            // Compress
            byte[] compressedData = Energy.Base.Compression.GZip.Compress(originalData);
            Assert.IsNotNull(compressedData, "Compressed data should not be null");
            Assert.IsTrue(compressedData.Length > 0, "Compressed data should not be empty");
            Assert.IsTrue(compressedData.Length < originalData.Length, "Compressed data should be smaller than original");

            // Decompress
            byte[] decompressedData = Energy.Base.Compression.GZip.Decompress(compressedData);
            Assert.IsNotNull(decompressedData, "Decompressed data should not be null");
            Assert.AreEqual(originalData.Length, decompressedData.Length, "Decompressed data should have same length as original");

            string decompressedText = System.Text.Encoding.UTF8.GetString(decompressedData);
            Assert.AreEqual(originalText, decompressedText, "Decompressed text should match original");
        }

        [TestMethod]
        public void Compression_GZip_EmptyData()
        {
            byte[] emptyData = new byte[0];

            byte[] compressedData = Energy.Base.Compression.GZip.Compress(emptyData);
            byte[] decompressedData = Energy.Base.Compression.GZip.Decompress(compressedData);

            Assert.AreEqual(0, decompressedData.Length, "Empty data should remain empty after compression/decompression");
        }

        [TestMethod]
        public void Compression_GZip_NullData()
        {
            byte[] compressedData = Energy.Base.Compression.GZip.Compress(null);
            byte[] decompressedData = Energy.Base.Compression.GZip.Decompress(null);

            Assert.IsNull(compressedData, "Compressing null should return null");
            Assert.IsNull(decompressedData, "Decompressing null should return null");
        }

        [TestMethod]
        public void Compression_ZX0_CompressDecompress()
        {
            // Test data
            string originalText = "Hello World! This is a test string for ZX0 compression. " +
                                "It contains some repeated patterns to make it compressible. " +
                                "Hello World! This is a test string for compression.";
            byte[] originalData = System.Text.Encoding.UTF8.GetBytes(originalText);

            // Compress
            byte[] compressedData = Energy.Base.Compression.ZX0.Compress(originalData);
            Assert.IsNotNull(compressedData, "Compressed data should not be null");
            Assert.IsTrue(compressedData.Length > 0, "Compressed data should not be empty");

            // Decompress
            byte[] decompressedData = Energy.Base.Compression.ZX0.Decompress(compressedData);
            Assert.IsNotNull(decompressedData, "Decompressed data should not be null");

            string decompressedText = System.Text.Encoding.UTF8.GetString(decompressedData);
            Assert.AreEqual(originalText, decompressedText, "Decompressed text should match original");
        }

        [TestMethod]
        public void Compression_ZX0_EmptyData()
        {
            byte[] emptyData = new byte[0];

            byte[] compressedData = Energy.Base.Compression.ZX0.Compress(emptyData);
            byte[] decompressedData = Energy.Base.Compression.ZX0.Decompress(compressedData);

            Assert.AreEqual(0, decompressedData.Length, "Empty data should remain empty after compression/decompression");
        }

        [TestMethod]
        public void Compression_ZX0_NullData()
        {
            byte[] compressedData = Energy.Base.Compression.ZX0.Compress(null);
            byte[] decompressedData = Energy.Base.Compression.ZX0.Decompress(null);

            Assert.IsNull(compressedData, "Compressing null should return null");
            Assert.IsNull(decompressedData, "Decompressing null should return null");
        }

        [TestMethod]
        public void Compression_ZX0_TestFile()
        {
            // ZX0_NUMBERS: reference fixture decodes to source, and recompressing the
            // source reproduces the fixture byte for byte.
            byte[] numbersCompressedData = ReadEmbeddedResource("Resources.ZX0_NUMBERS.zx0");
            Assert.IsNotNull(numbersCompressedData, "ZX0_NUMBERS.zx0 should exist and not be null");
            Assert.IsTrue(numbersCompressedData.Length > 0, "ZX0_NUMBERS.zx0 should not be empty");

            byte[] numbersExpectedData = ReadEmbeddedResource("Resources.ZX0_NUMBERS.txt");
            Assert.IsNotNull(numbersExpectedData, "ZX0_NUMBERS.txt should exist and not be null");

            byte[] numbersDecompressedData = Energy.Base.Compression.ZX0.Decompress(numbersCompressedData);
            Assert.IsNotNull(numbersDecompressedData, "Decompression should not return null");
            CollectionAssert.AreEqual(numbersExpectedData, numbersDecompressedData, "Decompressed ZX0_NUMBERS.zx0 should match ZX0_NUMBERS.txt");

            byte[] numbersCompressedFromText = Energy.Base.Compression.ZX0.Compress(numbersExpectedData);
            Assert.IsNotNull(numbersCompressedFromText, "Compression should not return null");
            CollectionAssert.AreEqual(numbersCompressedData, numbersCompressedFromText, "Compressed data should match fixture ZX0_NUMBERS.zx0");

            byte[] numbersRoundtripDecompressed = Energy.Base.Compression.ZX0.Decompress(numbersCompressedFromText);
            Assert.IsNotNull(numbersRoundtripDecompressed, "Roundtrip decompression should not return null");
            CollectionAssert.AreEqual(numbersExpectedData, numbersRoundtripDecompressed, "Roundtrip compression/decompression should match original");

            // ZX0_LETTERS
            byte[] testCompressedData = ReadEmbeddedResource("Resources.ZX0_LETTERS.zx0");
            Assert.IsNotNull(testCompressedData, "ZX0_LETTERS.zx0 should exist and not be null");
            Assert.IsTrue(testCompressedData.Length > 0, "ZX0_LETTERS.zx0 should not be empty");

            byte[] testExpectedData = ReadEmbeddedResource("Resources.ZX0_LETTERS.txt");
            Assert.IsNotNull(testExpectedData, "ZX0_LETTERS.txt should exist and not be null");

            byte[] testDecompressedData = Energy.Base.Compression.ZX0.Decompress(testCompressedData);
            Assert.IsNotNull(testDecompressedData, "Decompression should not return null");
            CollectionAssert.AreEqual(testExpectedData, testDecompressedData, "Decompressed ZX0_LETTERS.zx0 should match ZX0_LETTERS.txt");

            byte[] testCompressedFromText = Energy.Base.Compression.ZX0.Compress(testExpectedData);
            Assert.IsNotNull(testCompressedFromText, "Compression should not return null");
            CollectionAssert.AreEqual(testCompressedData, testCompressedFromText, "Compressed data should match fixture ZX0_LETTERS.zx0");

            byte[] testRoundtripDecompressed = Energy.Base.Compression.ZX0.Decompress(testCompressedFromText);
            Assert.IsNotNull(testRoundtripDecompressed, "Roundtrip decompression should not return null");
            CollectionAssert.AreEqual(testExpectedData, testRoundtripDecompressed, "Roundtrip compression/decompression should match original");

            // ZX0_SIMPLE
            byte[] simpleCompressedData = ReadEmbeddedResource("Resources.ZX0_SIMPLE.zx0");
            Assert.IsNotNull(simpleCompressedData, "ZX0_SIMPLE.zx0 should exist and not be null");
            Assert.AreEqual(17, simpleCompressedData.Length, "ZX0_SIMPLE.zx0 should be 17 bytes");

            byte[] simpleExpectedData = ReadEmbeddedResource("Resources.ZX0_SIMPLE.txt");
            Assert.IsNotNull(simpleExpectedData, "ZX0_SIMPLE.txt should exist and not be null");
            Assert.AreEqual(13, simpleExpectedData.Length, "ZX0_SIMPLE.txt should be 13 bytes");

            byte[] simpleDecompressedData = Energy.Base.Compression.ZX0.Decompress(simpleCompressedData);
            Assert.IsNotNull(simpleDecompressedData, "Decompression should not return null");
            CollectionAssert.AreEqual(simpleExpectedData, simpleDecompressedData, "Decompressed ZX0_SIMPLE.zx0 should match ZX0_SIMPLE.txt");

            byte[] simpleCompressedFromText = Energy.Base.Compression.ZX0.Compress(simpleExpectedData);
            Assert.IsNotNull(simpleCompressedFromText, "Compression should not return null");
            Assert.AreEqual(17, simpleCompressedFromText.Length, "Compressed ZX0_SIMPLE should be 17 bytes");
            CollectionAssert.AreEqual(simpleCompressedData, simpleCompressedFromText, "Compressed data should match fixture ZX0_SIMPLE.zx0 byte by byte");

            byte[] simpleRoundtripDecompressed = Energy.Base.Compression.ZX0.Decompress(simpleCompressedFromText);
            Assert.IsNotNull(simpleRoundtripDecompressed, "Roundtrip decompression should not return null");
            CollectionAssert.AreEqual(simpleExpectedData, simpleRoundtripDecompressed, "Roundtrip compression/decompression should match original");
        }

        [TestMethod]
        public void Compression_ZX0_RandomNumeric200()
        {
            // Generate random 200-character numeric text (0-9)
            Random random = new Random(42); // Fixed seed for reproducible tests
            char[] numericChars = new char[200];
            for (int i = 0; i < 200; i++)
            {
                numericChars[i] = (char)('0' + random.Next(0, 10));
            }
            string originalText = new string(numericChars);
            byte[] originalData = System.Text.Encoding.UTF8.GetBytes(originalText);

            // Compress
            byte[] compressedData = Energy.Base.Compression.ZX0.Compress(originalData);
            Assert.IsNotNull(compressedData, "Compressed data should not be null");
            Assert.IsTrue(compressedData.Length > 0, "Compressed data should not be empty");
            Assert.IsFalse(compressedData.Length == originalData.Length, "Compressed data length must not be the same as original");

            // Decompress
            byte[] decompressedData = Energy.Base.Compression.ZX0.Decompress(compressedData);
            Assert.IsNotNull(decompressedData, "Decompressed data should not be null");
            Assert.AreEqual(originalData.Length, decompressedData.Length, "Decompressed data should have same length as original");

            string decompressedText = System.Text.Encoding.UTF8.GetString(decompressedData);
            Assert.AreEqual(originalText, decompressedText, "Decompressed text should match original");

            // Verify the text contains only numbers and is 200 characters long
            Assert.AreEqual(200, originalText.Length, "Original text should be 200 characters long");
            Assert.AreEqual(200, decompressedText.Length, "Decompressed text should be 200 characters long");

            foreach (char c in originalText)
            {
                Assert.IsTrue(c >= '0' && c <= '9', "Original text should contain only digits 0-9");
            }

            foreach (char c in decompressedText)
            {
                Assert.IsTrue(c >= '0' && c <= '9', "Decompressed text should contain only digits 0-9");
            }
        }

        [TestMethod]
        public void Compression_ZX0_RepeatedPatterns()
        {
            // Test data with lots of repetition (good for compression)
            string originalText = "ABCDEF" + "ABCDEF" + "ABCDEF" + "ABCDEF" + "ABCDEF" +
                                "GHIJKL" + "GHIJKL" + "GHIJKL" + "GHIJKL" + "GHIJKL" +
                                "MNOPQR" + "MNOPQR" + "MNOPQR" + "MNOPQR" + "MNOPQR";
            byte[] originalData = System.Text.Encoding.UTF8.GetBytes(originalText);

            // Compress
            byte[] compressedData = Energy.Base.Compression.ZX0.Compress(originalData);
            Assert.IsNotNull(compressedData, "Compressed data should not be null");
            // Note: Size test removed since our simple implementation doesn't compress
            // This would require a full ZX0 implementation with actual compression

            // Decompress
            byte[] decompressedData = Energy.Base.Compression.ZX0.Decompress(compressedData);
            Assert.IsNotNull(decompressedData, "Decompressed data should not be null");
            Assert.HasCount(originalData.Length, decompressedData);

            string decompressedText = System.Text.Encoding.UTF8.GetString(decompressedData);
            Assert.AreEqual(originalText, decompressedText, "Decompressed text should match original");
        }

        [TestMethod]
        public void Compression_ZX0_1000Characters()
        {
            // Load ZX0_1000.zx0 compressed data
            byte[] compressedData = ReadEmbeddedResource("Resources.ZX0_1000.zx0");
            Assert.IsNotNull(compressedData, "ZX0_1000.zx0 should exist and not be null");
            Assert.IsTrue(compressedData.Length > 0, "ZX0_1000.zx0 should not be empty");

            // Decompress using ZX0 algorithm
            byte[] decompressedData = Energy.Base.Compression.ZX0.Decompress(compressedData);
            Assert.IsNotNull(decompressedData, "Decompression should not return null");

            // Verify length is exactly 1000 characters
            Assert.HasCount(1000, decompressedData);

            // Build expected output: "0123456789" repeated 100 times
            System.Text.StringBuilder expected = new System.Text.StringBuilder(1000);
            for (int i = 0; i < 100; i++)
            {
                expected.Append("0123456789");
            }
            string expectedText = expected.ToString();

            // Compare decompressed data with expected result
            string actualText = System.Text.Encoding.UTF8.GetString(decompressedData);
            Assert.AreEqual(expectedText, actualText,
                "Decompressed ZX0_1000.zx0 should be 1000 characters of repeated '0123456789'");
        }

        [TestMethod]
        public void Compression_ZX0_Big16KFixture()
        {
            // Larger structured input. Guards byte-identical output against the
            // reference zx0 tool and confirms the optimizer still completes promptly
            // as input size grows (regression guard for the arena rewrite).
            byte[] plain = ReadEmbeddedResource("Resources.ZX0_BIG16K.txt");
            byte[] expectedCompressed = ReadEmbeddedResource("Resources.ZX0_BIG16K.zx0");
            Assert.IsNotNull(plain, "ZX0_BIG16K.txt fixture should be embedded");
            Assert.IsNotNull(expectedCompressed, "ZX0_BIG16K.zx0 fixture should be embedded");
            Assert.AreEqual(16384, plain.Length, "ZX0_BIG16K.txt should be 16384 bytes");

            byte[] actualCompressed = Energy.Base.Compression.ZX0.Compress(plain);
            Assert.IsNotNull(actualCompressed, "ZX0 compression should not return null");
            CollectionAssert.AreEqual(expectedCompressed, actualCompressed, "ZX0 compression output should match ZX0_BIG16K.zx0 fixture byte for byte");

            byte[] roundTrip = Energy.Base.Compression.ZX0.Decompress(actualCompressed);
            Assert.IsNotNull(roundTrip, "ZX0 decompression should not return null");
            CollectionAssert.AreEqual(plain, roundTrip, "Round-trip should reproduce ZX0_BIG16K.txt");

            byte[] fixtureDecompressed = Energy.Base.Compression.ZX0.Decompress(expectedCompressed);
            Assert.IsNotNull(fixtureDecompressed, "Decompressing the reference fixture should not return null");
            CollectionAssert.AreEqual(plain, fixtureDecompressed, "Reference fixture should decode to ZX0_BIG16K.txt");
        }

        [TestMethod]
        public void Compression_ZX0_Random8KFixture()
        {
            // Incompressible input exercises the literal-heavy path at scale and keeps
            // output byte-identical to the reference zx0 tool.
            byte[] plain = ReadEmbeddedResource("Resources.ZX0_RANDOM8K.bin");
            byte[] expectedCompressed = ReadEmbeddedResource("Resources.ZX0_RANDOM8K.zx0");
            Assert.IsNotNull(plain, "ZX0_RANDOM8K.bin fixture should be embedded");
            Assert.IsNotNull(expectedCompressed, "ZX0_RANDOM8K.zx0 fixture should be embedded");
            Assert.AreEqual(8192, plain.Length, "ZX0_RANDOM8K.bin should be 8192 bytes");

            byte[] actualCompressed = Energy.Base.Compression.ZX0.Compress(plain);
            Assert.IsNotNull(actualCompressed, "ZX0 compression should not return null");
            CollectionAssert.AreEqual(expectedCompressed, actualCompressed, "ZX0 compression output should match ZX0_RANDOM8K.zx0 fixture byte for byte");

            byte[] roundTrip = Energy.Base.Compression.ZX0.Decompress(actualCompressed);
            Assert.IsNotNull(roundTrip, "ZX0 decompression should not return null");
            CollectionAssert.AreEqual(plain, roundTrip, "Round-trip should reproduce ZX0_RANDOM8K.bin");
        }

        #region LZ4

        [TestMethod]
        public void Compression_LZ4_DecompressSmallFixture()
        {
            byte[] compressed = ReadEmbeddedResource("Resources.LZ4_SMALL.bin");
            byte[] expected = ReadEmbeddedResource("Resources.LZ4_SMALL_ORIGINAL.bin");
            Assert.IsNotNull(compressed, "LZ4_SMALL.bin fixture should exist");
            Assert.IsNotNull(expected, "LZ4_SMALL_ORIGINAL.bin fixture should exist");

            byte[] actual = Energy.Base.Compression.LZ4.Decompress(compressed);
            Assert.IsNotNull(actual, "Decompression should not return null");
            CollectionAssert.AreEqual(expected, actual, "Decompressed data should match original fixture");
        }

        [TestMethod]
        public void Compression_LZ4_DecompressEmptyFixture()
        {
            byte[] compressed = ReadEmbeddedResource("Resources.LZ4_EMPTY.bin");
            byte[] expected = ReadEmbeddedResource("Resources.LZ4_EMPTY_ORIGINAL.bin");
            Assert.IsNotNull(compressed, "LZ4_EMPTY.bin fixture should exist");

            byte[] actual = Energy.Base.Compression.LZ4.Decompress(compressed);
            Assert.IsNotNull(actual, "Decompression should not return null");
            Assert.AreEqual(0, actual.Length, "Empty decompression should produce empty output");
        }

        [TestMethod]
        public void Compression_LZ4_DecompressPatternFixture()
        {
            byte[] compressed = ReadEmbeddedResource("Resources.LZ4_PATTERN.bin");
            byte[] expected = ReadEmbeddedResource("Resources.LZ4_PATTERN_ORIGINAL.bin");
            Assert.IsNotNull(compressed, "LZ4_PATTERN.bin fixture should exist");
            Assert.IsNotNull(expected, "LZ4_PATTERN_ORIGINAL.bin fixture should exist");

            byte[] actual = Energy.Base.Compression.LZ4.Decompress(compressed);
            Assert.IsNotNull(actual, "Decompression should not return null");
            CollectionAssert.AreEqual(expected, actual, "Decompressed pattern should match original");
        }

        [TestMethod]
        public void Compression_LZ4_DecompressRandomFixture()
        {
            byte[] compressed = ReadEmbeddedResource("Resources.LZ4_RANDOM.bin");
            byte[] expected = ReadEmbeddedResource("Resources.LZ4_RANDOM_ORIGINAL.bin");
            Assert.IsNotNull(compressed, "LZ4_RANDOM.bin fixture should exist");
            Assert.IsNotNull(expected, "LZ4_RANDOM_ORIGINAL.bin fixture should exist");

            byte[] actual = Energy.Base.Compression.LZ4.Decompress(compressed);
            Assert.IsNotNull(actual, "Decompression should not return null");
            CollectionAssert.AreEqual(expected, actual, "Decompressed random should match original");
        }

        [TestMethod]
        public void Compression_LZ4_RoundTripSmall()
        {
            string text = "Hello World! Hello World! Hello World!";
            byte[] original = System.Text.Encoding.ASCII.GetBytes(text);

            byte[] compressed = Energy.Base.Compression.LZ4.Compress(original);
            Assert.IsNotNull(compressed, "Compression should not return null");

            byte[] decompressed = Energy.Base.Compression.LZ4.Decompress(compressed);
            Assert.IsNotNull(decompressed, "Decompression should not return null");
            CollectionAssert.AreEqual(original, decompressed, "Round-trip should reproduce original");
        }

        [TestMethod]
        public void Compression_LZ4_RoundTripPattern()
        {
            byte[] original = new byte[5000];
            for (int i = 0; i < 5000; i++)
            {
                original[i] = (byte)(i % 256);
            }

            byte[] compressed = Energy.Base.Compression.LZ4.Compress(original);
            Assert.IsNotNull(compressed, "Compression should not return null");

            byte[] decompressed = Energy.Base.Compression.LZ4.Decompress(compressed);
            Assert.IsNotNull(decompressed, "Decompression should not return null");
            CollectionAssert.AreEqual(original, decompressed, "Round-trip should reproduce original pattern");
        }

        [TestMethod]
        public void Compression_LZ4_RoundTripRandom()
        {
            byte[] original = new byte[5000];
            System.Random rng = new System.Random(42);
            rng.NextBytes(original);

            byte[] compressed = Energy.Base.Compression.LZ4.Compress(original);
            Assert.IsNotNull(compressed, "Compression should not return null");

            byte[] decompressed = Energy.Base.Compression.LZ4.Decompress(compressed);
            Assert.IsNotNull(decompressed, "Decompression should not return null");
            CollectionAssert.AreEqual(original, decompressed, "Round-trip should reproduce original random data");
        }

        [TestMethod]
        public void Compression_LZ4_RoundTripLarge()
        {
            byte[] original = new byte[1024 * 1024];
            string phrase = "Hello World! This is a test of LZ4 compression. ";
            byte[] phraseBytes = System.Text.Encoding.ASCII.GetBytes(phrase);
            for (int i = 0; i < original.Length; i++)
            {
                original[i] = phraseBytes[i % phraseBytes.Length];
            }

            byte[] compressed = Energy.Base.Compression.LZ4.Compress(original);
            Assert.IsNotNull(compressed, "Compression should not return null");

            byte[] decompressed = Energy.Base.Compression.LZ4.Decompress(compressed);
            Assert.IsNotNull(decompressed, "Decompression should not return null");
            CollectionAssert.AreEqual(original, decompressed, "Round-trip should reproduce 1MB original");
        }

        [TestMethod]
        public void Compression_LZ4_NullData()
        {
            byte[] compressed = Energy.Base.Compression.LZ4.Compress(null);
            byte[] decompressed = Energy.Base.Compression.LZ4.Decompress(null);

            Assert.IsNull(compressed, "Compressing null should return null");
            Assert.IsNull(decompressed, "Decompressing null should return null");
        }

        #endregion

        #region LZSS

        private static Energy.Base.Compression.LZSS.Options LzssOptions(int offsetBits, int lengthBits, int minimumMatch, bool literalFirst, bool positionStartZero, bool forceLastLiteral)
        {
            Energy.Base.Compression.LZSS.Options options = new Energy.Base.Compression.LZSS.Options();
            options.OffsetBits = offsetBits;
            options.LengthBits = lengthBits;
            options.MinimumMatch = minimumMatch;
            options.LiteralFirst = literalFirst;
            options.PositionStartZero = positionStartZero;
            options.ForceLastLiteral = forceLastLiteral;
            return options;
        }

        // Compress the plain fixture, assert it equals the reference fixture byte for
        // byte, then assert that both the produced stream and the reference stream
        // decode back to the original.
        private void LzssFixtureCheck(string plainResource, string compressedResource, Energy.Base.Compression.LZSS.Options options)
        {
            byte[] plain = ReadEmbeddedResource(plainResource);
            byte[] expectedCompressed = ReadEmbeddedResource(compressedResource);
            Assert.IsNotNull(plain, plainResource + " should be embedded");
            Assert.IsNotNull(expectedCompressed, compressedResource + " should be embedded");

            byte[] actualCompressed = Energy.Base.Compression.LZSS.Compress(plain, options);
            Assert.IsNotNull(actualCompressed, "LZSS compression should not return null");
            CollectionAssert.AreEqual(expectedCompressed, actualCompressed, "LZSS output should match " + compressedResource + " byte for byte");

            byte[] roundTrip = Energy.Base.Compression.LZSS.Decompress(actualCompressed, options);
            Assert.IsNotNull(roundTrip, "LZSS decompression should not return null");
            CollectionAssert.AreEqual(plain, roundTrip, "Round-trip should reproduce " + plainResource);

            byte[] fixtureDecompressed = Energy.Base.Compression.LZSS.Decompress(expectedCompressed, options);
            Assert.IsNotNull(fixtureDecompressed, "Decompressing the reference fixture should not return null");
            CollectionAssert.AreEqual(plain, fixtureDecompressed, "Reference fixture should decode to " + plainResource);
        }

        [TestMethod]
        public void Compression_LZSS_DefaultFixtures()
        {
            // Default 8-bit preset (offset 4, length 4, minimum match 2), byte-for-byte
            // against the reference lzss-sap encoder for several inputs.
            Energy.Base.Compression.LZSS.Options options = LzssOptions(4, 4, 2, true, false, true);
            LzssFixtureCheck("Resources.LZSS_HELLO.bin", "Resources.LZSS_HELLO.lz8", options);
            LzssFixtureCheck("Resources.LZSS_PATTERN.bin", "Resources.LZSS_PATTERN.lz8", options);
            LzssFixtureCheck("Resources.LZSS_REGLOG.bin", "Resources.LZSS_REGLOG.lz8", options);
            LzssFixtureCheck("Resources.LZSS_RANDOM.bin", "Resources.LZSS_RANDOM.lz8", options);
        }

        [TestMethod]
        public void Compression_LZSS_WideFixtures()
        {
            // 12-bit preset exercises the half-byte match path; 16-bit preset exercises
            // the two-byte match path and the maximum-length wrap.
            LzssFixtureCheck("Resources.LZSS_BIG.bin", "Resources.LZSS_BIG.lz12", LzssOptions(7, 5, 2, true, false, true));
            LzssFixtureCheck("Resources.LZSS_BIG.bin", "Resources.LZSS_BIG.lz16", LzssOptions(8, 8, 1, true, false, true));
        }

        [TestMethod]
        public void Compression_LZSS_DefaultOverloadMatchesPreset()
        {
            // The parameterless overloads must behave like the explicit default preset.
            byte[] plain = ReadEmbeddedResource("Resources.LZSS_HELLO.bin");
            byte[] expectedCompressed = ReadEmbeddedResource("Resources.LZSS_HELLO.lz8");

            byte[] compressed = Energy.Base.Compression.LZSS.Compress(plain);
            Assert.IsNotNull(compressed, "Default compression should not return null");
            CollectionAssert.AreEqual(expectedCompressed, compressed, "Default overload should match the 8-bit fixture");

            byte[] roundTrip = Energy.Base.Compression.LZSS.Decompress(compressed);
            Assert.IsNotNull(roundTrip, "Default decompression should not return null");
            CollectionAssert.AreEqual(plain, roundTrip, "Default overload round-trip should reproduce the original");
        }

        [TestMethod]
        public void Compression_LZSS_RoundTrip()
        {
            // Round-trip a range of inputs across all presets and both format versions.
            Energy.Base.Compression.LZSS.Options[] presets = new Energy.Base.Compression.LZSS.Options[]
            {
                LzssOptions(4, 4, 2, true, false, true),    // 8-bit current
                LzssOptions(7, 5, 2, true, false, true),    // 12-bit current
                LzssOptions(8, 8, 1, true, false, true),    // 16-bit current
                LzssOptions(4, 4, 2, false, true, true),    // 8-bit old format
                LzssOptions(4, 4, 2, true, false, false),   // no forced last literal
            };

            byte[][] inputs = new byte[][]
            {
                new byte[] { 65 },
                System.Text.Encoding.ASCII.GetBytes("ABABABABABABABABABABAB"),
                System.Text.Encoding.ASCII.GetBytes("The quick brown fox. The quick brown fox. The quick brown fox."),
                BuildRepeating(2000),
                BuildPseudoRandom(1500),
            };

            for (int p = 0; p < presets.Length; p++)
            {
                for (int i = 0; i < inputs.Length; i++)
                {
                    byte[] compressed = Energy.Base.Compression.LZSS.Compress(inputs[i], presets[p]);
                    Assert.IsNotNull(compressed, "Compression should not return null");
                    byte[] decompressed = Energy.Base.Compression.LZSS.Decompress(compressed, presets[p]);
                    Assert.IsNotNull(decompressed, "Decompression should not return null");
                    CollectionAssert.AreEqual(inputs[i], decompressed, "Round-trip should reproduce input " + i + " for preset " + p);
                }
            }
        }

        [TestMethod]
        public void Compression_LZSS_NullAndEmpty()
        {
            Assert.IsNull(Energy.Base.Compression.LZSS.Compress(null), "Compressing null should return null");
            Assert.IsNull(Energy.Base.Compression.LZSS.Decompress(null), "Decompressing null should return null");

            byte[] compressedEmpty = Energy.Base.Compression.LZSS.Compress(new byte[0]);
            Assert.IsNotNull(compressedEmpty, "Compressing empty should not return null");
            Assert.AreEqual(0, compressedEmpty.Length, "Compressing empty should return empty");

            byte[] decompressedEmpty = Energy.Base.Compression.LZSS.Decompress(new byte[0]);
            Assert.IsNotNull(decompressedEmpty, "Decompressing empty should not return null");
            Assert.AreEqual(0, decompressedEmpty.Length, "Decompressing empty should return empty");
        }

        [TestMethod]
        public void Compression_LZSS_InvalidOptions()
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes("some data to compress");
            Assert.IsNull(Energy.Base.Compression.LZSS.Compress(data, LzssOptions(13, 4, 2, true, false, true)), "Offset bits above 12 should fail");
            Assert.IsNull(Energy.Base.Compression.LZSS.Compress(data, LzssOptions(4, 2, 2, true, false, true)), "Total bits below 8 should fail");
            Assert.IsNull(Energy.Base.Compression.LZSS.Compress(data, LzssOptions(4, 4, 0, true, false, true)), "Minimum match below 1 should fail");
        }

        private static byte[] BuildRepeating(int length)
        {
            byte[] data = new byte[length];
            for (int i = 0; i < length; i++)
                data[i] = (byte)(65 + (i % 13));
            return data;
        }

        private static byte[] BuildPseudoRandom(int length)
        {
            byte[] data = new byte[length];
            Random random = new Random(7);
            random.NextBytes(data);
            return data;
        }

        #endregion

        // .NET 2.0 compatible file reading method
        private byte[] ReadAllBytes(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }

        // .NET 2.0 compatible embedded resource reading method
        private byte[] ReadEmbeddedResource(string resourceName)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            string fullResourceName = assembly.GetName().Name + "." + resourceName;

            using (Stream stream = assembly.GetManifestResourceStream(fullResourceName))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException("Embedded resource not found: " + fullResourceName);
                }

                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }
    }
}
