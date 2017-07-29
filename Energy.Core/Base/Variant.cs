using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Energy.Base
{
    public class Variant
    {
        #region Union

        public struct Union
        {
            public object Value;

            public bool Bool
            {
                get
                {
                    if (Value == null)
                        return false;
                    if (Value is bool)
                        return (bool)Value;
                    return Energy.Base.Cast.ObjectToBool(Value);
                }
                set { Value = value; }
            }

            public byte UInt8
            {
                get
                {
                    if (Value == null)
                        return (byte)0;
                    if (Value is byte)
                        return (byte)Value;
                    return Energy.Base.Cast.ObjectToByte(Value);
                }
                set { Value = value; }
            }

            public SByte SInt8
            {
                get { return Value == null ? (SByte)0 : (SByte)Value; }
                set { Value = value; }
            }

            public UInt16 UInt16
            {
                get { return Value == null ? (UInt16)0 : (UInt16)Value; }
                set { Value = value; }
            }

            public Int16 SInt16
            {
                get { return Value == null ? (Int16)0 : (Int16)Value; }
                set { Value = value; }
            }

            public UInt32 UInt32
            {
                get { return Value == null ? (UInt16)0 : (UInt16)Value; }
                set { Value = value; }
            }

            public Int32 SInt32
            {
                get { return Value == null ? (Int32)0 : (Int32)Value; }
                set { Value = value; }
            }

            public UInt64 UInt64
            {
                get { return Value == null ? (UInt64)0 : (UInt64)Value; }
                set { Value = value; }
            }

            public Int64 SInt64
            {
                get { return Value == null ? (Int64)0 : (Int64)Value; }
                set { Value = value; }
            }

            public char Char
            {
                get { return Value == null ? (char)0 : (char)Value; }
                set { Value = value; }
            }

            public float Float
            {
                get { return Value == null ? (char)0 : (char)Value; }
                set { Value = value; }
            }

            public double Double
            {
                get { return Value == null ? (double)0 : (double)Value; }
                set { Value = value; }
            }

            public decimal Decimal
            {
                get { return Value == null ? (decimal)0 : (decimal)Value; }
                set { Value = value; }
            }

            public DateTime DateTime
            {
                get { return Value == null ? System.DateTime.MinValue : (System.DateTime)Value; }
                set { Value = value; }
            }

            public TimeSpan TimeSpan
            {
                get { return Value == null ? System.TimeSpan.MinValue : (System.TimeSpan)Value; }
                set { Value = value; }
            }

            public string String
            {
                get { return (string)Value; }
                set { Value = value; }
            }

            public object[] Array
            {
                get { return (object[])Value; }
                set { Value = value; }
            }

            public object Object
            {
                get { return (object)Value; }
                set { Value = value; }
            }
        }

        #endregion

        #region Explicit union

        [StructLayout(LayoutKind.Explicit, Size = 64)]
        public struct UnionExplicit
        {
            [FieldOffset(0)]
            public bool Bool;

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

        #endregion

        #region Type

        public enum Type
        {
            Null,
            Bool,
            UInt8,
            SInt8,
            UInt16,
            SInt16,
            UInt32,
            SInt32,
            UInt64,
            SInt64,
            Char,
            Float,
            Double,
            Decimal,
            DateTime,
            String,
            Array,
            Object,
        }

        #endregion

        #region Value

        public class Value
        {
            private Union _Union;
            /// <summary>Union</summary>
            public Union Union { get { return _Union; } set { _Union = value; } }

            public Type Type;

            public Value(object value)
            {
                this.Object = value;
            }

            public static explicit operator string(Value o)
            {
                switch (o.Type)
                {
                    case Type.Char:
                        return ((object)o).ToString();
                    case Type.DateTime:
                        return Energy.Base.Cast.DateTimeToString(o.Union.DateTime);
                    default:
                        return null;
                }
            }

            public override string ToString()
            {
                return (string)this;
            }

            public object Object
            {
                get
                {
                    return GetObject();
                }
                set
                {
                    SetObject(value);
                }
            }

            private void SetObject(object value)
            {
                if (value == null)
                {
                    this.Type = Type.Null;
                    return;
                }
                if (value is bool)
                {
                    this.Type = Type.Bool;
                    //this.Union = new Union() { Bool = (bool)value ? (byte)1 : (byte)0 };
                    this.Union = new Union() { Bool = (bool)value };
                    return;
                }
                if (value is byte)
                {
                    this.Type = Type.UInt8;
                    this.Union = new Union() { UInt8 = (byte)value };
                    return;
                }
                if (value is UInt16)
                {
                    this.Type = Type.UInt16;
                    this.Union = new Union() { UInt16 = (UInt16)value };
                    return;
                }
                if (value is UInt32)
                {
                    this.Type = Type.UInt32;
                    this.Union = new Union() { UInt32 = (UInt32)value };
                    return;
                }
                if (value is UInt64)
                {
                    this.Type = Type.UInt64;
                    this.Union = new Union() { UInt64 = (UInt64)value };
                    return;
                }
                if (value is sbyte)
                {
                    this.Type = Type.UInt8;
                    this.Union = new Union() { SInt8 = (sbyte)value };
                    return;
                }
                if (value is Int16)
                {
                    this.Type = Type.SInt16;
                    this.Union = new Union() { SInt16 = (Int16)value };
                    return;
                }
                if (value is int)
                {
                    this.Type = Type.SInt32;
                    this.Union = new Union() { SInt32 = (int)value };
                    return;
                }
                if (value is Int64)
                {
                    this.Type = Type.SInt64;
                    this.Union = new Union() { SInt64 = (Int64)value };
                    return;
                }
                if (value is float)
                {
                    this.Type = Type.Float;
                    this.Union = new Union() { Float = (float)value };
                    return;
                }
                if (value is double)
                {
                    this.Type = Type.Double;
                    this.Union = new Union() { Double = (double)value };
                    return;
                }
                if (value is decimal)
                {
                    this.Type = Type.Decimal;
                    this.Union = new Union() { Decimal = (decimal)value };
                    return;
                }
                if (value is char)
                {
                    this.Type = Type.Char;
                    this.Union = new Union() { Char = (char)value };
                    return;
                }
                if (value is DateTime)
                {
                    this.Type = Type.DateTime;
                    this.Union = new Union() { DateTime = (DateTime)value };
                    return;
                }
                if (value is string)
                {
                    this.Type = Type.String;
                    this.Union = new Union() { String = (string)value };
                    return;
                }
                if (value.GetType().IsArray)
                {
                    this.Type = Type.Array;
                    this.Union = new Union() { Array = (object[])value };
                    return;
                }
                if (value is object)
                {
                    this.Type = Type.Object;
                    this.Union = new Union() { Object = value };
                    return;
                }
            }

            private object GetObject()
            {
                switch (this.Type)
                {
                    default:
                    case Type.Null:
                        return null;
                    case Type.Bool:
                        return this.Union.Bool;
                    case Type.UInt8:
                        return this.Union.UInt8;
                    case Type.UInt16:
                        return this.Union.UInt16;
                    case Type.UInt32:
                        return this.Union.UInt32;
                    case Type.UInt64:
                        return this.Union.UInt64;
                    case Type.SInt8:
                        return this.Union.SInt8;
                    case Type.SInt16:
                        return this.Union.SInt16;
                    case Type.SInt32:
                        return this.Union.SInt32;
                    case Type.SInt64:
                        return this.Union.SInt64;
                    case Type.Char:
                        return this.Union.Char;
                    case Type.Float:
                        return this.Union.Float;
                    case Type.Double:
                        return this.Union.Double;
                    case Type.Decimal:
                        return this.Union.Decimal;
                    case Type.DateTime:
                        return this.Union.DateTime;
                    case Type.String:
                        return this.Union.String;
                    case Type.Array:
                        return this.Union.Array;
                    case Type.Object:
                        return this.Union.Object;
                }
            }
        }

        #endregion

        #region Variable

        public class Variable
        {
            public string Name;

            public Value Value;
        }

        #endregion

        #region Dictionary

        public class Dictionary: Energy.Base.Collection.StringDictionary<Value>
        {

        }

        #endregion
    }
}
