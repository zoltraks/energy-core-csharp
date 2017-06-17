using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Energy.Base
{
    public class Variant
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct Union
        {
            [FieldOffset(0)]
            public byte UInt8;

            [FieldOffset(0)]
            public SByte SInt8;

            [FieldOffset(0)]
            public UInt16 UInt16;

            [FieldOffset(0)]
            public Int16 SInt16;

            [FieldOffset(0)]
            public UInt32 UInt32;

            [FieldOffset(0)]
            public Int32 SInt32;

            [FieldOffset(0)]
            public UInt64 UInt64;

            [FieldOffset(0)]
            public Int64 SInt64;

            [FieldOffset(0)]
            public float Float;

            [FieldOffset(0)]
            public double Double;

            [FieldOffset(0)]
            public decimal Decimal;

            [FieldOffset(0)]
            public DateTime DateTime;

            [FieldOffset(0)]
            public char Char;

            [FieldOffset(0)]
            public string String;

            [FieldOffset(0)]
            public object Object;

            [FieldOffset(0)]
            public object[] Array;
        }

        public enum Type
        {
            UInt8,
            SInt8,
            UInt16,
            SInt16,
            UInt32,
            SInt32,
            UInt64,
            SInt64,
            Float,
            Double,
            Decimal,
            DateTime,
            Char,
            String,
            Object,
            Array,
        }
    }
}
