using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Base
{
    /// <summary>
    /// Characterisation tests for Energy.Base.Variant.
    /// <br/><br/>
    /// These lock the current type detection and value round-trip behaviour of
    /// Energy.Base.Variant.Value so later refactoring can prove behaviour preservation.
    /// </summary>
    [TestClass]
    public class Variant
    {
        [TestMethod]
        public void Variant_Value_TypeDetection()
        {
            Assert.AreEqual(Energy.Base.Variant.Type.Null, new Energy.Base.Variant.Value(null).Type);
            Assert.AreEqual(Energy.Base.Variant.Type.Bool, new Energy.Base.Variant.Value(true).Type);
            Assert.AreEqual(Energy.Base.Variant.Type.UInt8, new Energy.Base.Variant.Value((byte)5).Type);
            Assert.AreEqual(Energy.Base.Variant.Type.UInt16, new Energy.Base.Variant.Value((ushort)5).Type);
            Assert.AreEqual(Energy.Base.Variant.Type.UInt32, new Energy.Base.Variant.Value((uint)5).Type);
            Assert.AreEqual(Energy.Base.Variant.Type.UInt64, new Energy.Base.Variant.Value((ulong)5).Type);
            Assert.AreEqual(Energy.Base.Variant.Type.SInt16, new Energy.Base.Variant.Value((short)5).Type);
            Assert.AreEqual(Energy.Base.Variant.Type.SInt32, new Energy.Base.Variant.Value((int)5).Type);
            Assert.AreEqual(Energy.Base.Variant.Type.SInt64, new Energy.Base.Variant.Value((long)5).Type);
            Assert.AreEqual(Energy.Base.Variant.Type.Float, new Energy.Base.Variant.Value((float)5).Type);
            Assert.AreEqual(Energy.Base.Variant.Type.Double, new Energy.Base.Variant.Value((double)5).Type);
            Assert.AreEqual(Energy.Base.Variant.Type.Decimal, new Energy.Base.Variant.Value((decimal)5).Type);
            Assert.AreEqual(Energy.Base.Variant.Type.Char, new Energy.Base.Variant.Value('x').Type);
            Assert.AreEqual(Energy.Base.Variant.Type.String, new Energy.Base.Variant.Value("text").Type);
            Assert.AreEqual(Energy.Base.Variant.Type.DateTime, new Energy.Base.Variant.Value(DateTime.Now).Type);
        }

        [TestMethod]
        public void Variant_Value_ObjectRoundTrip()
        {
            Assert.AreEqual(123, new Energy.Base.Variant.Value(123).Object);
            Assert.AreEqual("hello", new Energy.Base.Variant.Value("hello").Object);
            Assert.AreEqual(true, new Energy.Base.Variant.Value(true).Object);
            Assert.IsNull(new Energy.Base.Variant.Value(null).Object);
        }

        [TestMethod]
        public void Variant_Value_StringConversion()
        {
            Energy.Base.Variant.Value value = new Energy.Base.Variant.Value("sample");
            Assert.AreEqual("sample", (string)value);
            Assert.AreEqual("sample", value.ToString());

            // The explicit string conversion yields null for non-text types.
            Assert.IsNull((string)new Energy.Base.Variant.Value(42));
        }
    }
}
