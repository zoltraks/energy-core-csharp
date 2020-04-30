using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security.Policy;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Cast
    {
        private class Class
        {
            public class ExampleTypeStructure
            {
                private Int64? _NInt64;
                public Int64? NInt64 { get { return _NInt64; } set { _NInt64 = value; } }
            }

            public class TextRepresentation
            {
                private string _Text;
                /// <summary>Text</summary>
                public string Text { get { return _Text; } set { _Text = value; } }

                public override string ToString()
                {
                    return _Text;
                }
            }
        }

        [TestMethod]
        public void CastStringToDouble()
        {
            string str1 = "15,001";
            double num1 = Energy.Base.Cast.StringToDouble(str1);
            Assert.AreEqual(15.001, num1);
            num1 = Energy.Base.Cast.ObjectToDouble(str1);
            Assert.AreEqual(15.001, num1);
            string str2 = " 15.000000001 ";
            double num2 = Energy.Base.Cast.StringToDouble(str2);
            Assert.AreEqual(15.000000001, num2);
            num2 = Energy.Base.Cast.ObjectToDouble(str2);
            Assert.AreEqual(15.000000001, num2);
            string str3 = " -1,234 ";
            double num3 = Energy.Base.Cast.StringToDouble(str3);
            Assert.AreEqual(-1.234, num3);
            num3 = Energy.Base.Cast.ObjectToDouble(str3);
            Assert.AreEqual(-1.234, num3);
        }

        [TestMethod]
        public void CastObjectToDouble()
        {
            string s;
            object o;
            double n, x;
            byte? bn;
            s = " 15,001 ";
            o = s;
            x = 15.001;
            n = Energy.Base.Cast.ObjectToDouble(o);
            Assert.AreEqual(x, n);
            bn = 255;
            o = bn;
            x = 255;
            n = Energy.Base.Cast.ObjectToDouble(o);
            Assert.AreEqual(x, n);
            o = System.DBNull.Value;
            x = 0;
            n = Energy.Base.Cast.ObjectToDouble(o);
            Assert.AreEqual(x, n);
        }

        [TestMethod]
        public void CastObjectToDecimal()
        {
            string s;
            object o;
            decimal d;

            s = "15,0001";
            d = Energy.Base.Cast.StringToDecimal(s);
            Assert.AreEqual(15.0001m, d);

            s = "15.0001";
            d = Energy.Base.Cast.StringToDecimal(s);
            Assert.AreEqual(15.0001m, d);

            o = 15.0001f;
            d = Energy.Base.Cast.ObjectToDecimal(s);
            Assert.AreEqual(15.0001m, d);

            o = "15,0001";
            d = Energy.Base.Cast.ObjectToDecimal(s);
            Assert.AreEqual(15.0001m, d);
        }

        [TestMethod]
        public void CastStringToInteger()
        {
            string str1 = "15,001";
            int num1 = Energy.Base.Cast.StringToInteger(str1);
            Assert.AreEqual(15, num1);
            int num101 = Energy.Base.Cast.StringToInteger(str1, false);
            Assert.AreEqual(0, num101);
            string str2 = " 15.000000001 ";
            double num2 = Energy.Base.Cast.StringToInteger(str2);
            Assert.AreEqual(15, num2);
            string str3 = " -1,234 ";
            double num3 = Energy.Base.Cast.StringToInteger(str3);
            Assert.AreEqual(-1, num3);
            string str4 = " 81985529216486895 ";
            double num4 = Energy.Base.Cast.StringToInteger(str4);
            Assert.AreEqual(0, num4);
            string str5 = int.MaxValue.ToString();
            double num5 = Energy.Base.Cast.StringToInteger(str5);
            Assert.AreEqual(int.MaxValue, num5);
            string str6 = int.MinValue.ToString();
            double num6 = Energy.Base.Cast.StringToInteger(str6);
            Assert.AreEqual(int.MinValue, num6);
            string str7 = long.MaxValue.ToString();
            double num7 = Energy.Base.Cast.StringToInteger(str7);
            Assert.AreEqual(0, num7);
            string str8 = " 987654321 ";
            int num8 = Energy.Base.Cast.StringToInteger(str8);
            Assert.AreEqual(987654321, num8);
        }

        [TestMethod]
        public void CastObjectToInteger()
        {
            object source;
            int result;
            int expect;
            int? someNullable = null;
            source = someNullable;
            expect = 0;
            result = Energy.Base.Cast.AsInteger(source);
            Assert.AreEqual(expect, result);
            someNullable = 1;
            source = someNullable;
            expect = 1;
            result = Energy.Base.Cast.AsInteger(source);
            Assert.AreEqual(expect, result);
            source = "12345678,12345";
            expect = 12345678;
            result = Energy.Base.Cast.AsInteger(source);
            Assert.AreEqual(expect, result);
            source = 12345678901234567890m;
            expect = 0;
            result = Energy.Base.Cast.ObjectToInteger(source);
            Assert.AreEqual(expect, result);
            source = "12345678901234567890";
            expect = 0;
            result = Energy.Base.Cast.ObjectToInteger(source);
            Assert.AreEqual(expect, result);
        }

        [TestMethod]
        public void CastStringToDoubleSmart()
        {
            string str1 = "15,001_002";
            double num1 = Energy.Base.Cast.StringToDoubleSmart(str1);
            Assert.AreEqual(15.001002, num1);
            string str2 = " 15.000_000_001 ";
            double num2 = Energy.Base.Cast.StringToDoubleSmart(str2);
            Assert.AreEqual(15.000000001, num2);
            string str3 = " -1'000,234 ";
            double num3 = Energy.Base.Cast.StringToDoubleSmart(str3);
            Assert.AreEqual(-1000.234, num3);
        }

        [TestMethod]
        public void StringToBase64()
        {
            string s1 = "Abc";
            Assert.AreEqual("QWJj", Energy.Base.Cast.StringToBase64(s1));
            string s2 = "Gęś";
            Assert.AreEqual("R8SZxZs=", Energy.Base.Cast.StringToBase64(s2));
        }

        [TestMethod]
        public void Base64ToString()
        {
            string s1 = "TWljcm9zb2Z0IFdpbmRvd3M=";
            Assert.AreEqual("Microsoft Windows", Energy.Base.Cast.Base64ToString(s1));
            string s2 = "R8SZxZs=";
            Assert.AreEqual("Gęś", Energy.Base.Cast.Base64ToString(s2));
            // wrong Base64
            string s3 = null;
            Assert.AreEqual(null, Energy.Base.Cast.Base64ToString(s3));
            string s4 = "";
            Assert.AreEqual("", Energy.Base.Cast.Base64ToString(s4));
            string s5 = "=";
            Assert.AreEqual(null, Energy.Base.Cast.Base64ToString(s5));
            // wrong much more with valid BASE64 but invalid UTF-8
            byte[] b1 = new byte[] { 0xfe, 0xff, 0xff, 0xf0, 0x90, 0xbc };
            string s6 = Convert.ToBase64String(b1);
            string s7 = Energy.Base.Cast.Base64ToString(s6);
            byte[] b2 = System.Text.Encoding.Unicode.GetBytes(s7);
            byte[] b3 = new byte[] { 0xfd, 0xff, 0xfd, 0xff, 0xfd, 0xff, 0xfd, 0xff };
            int c1 = Energy.Base.Bit.Compare(b2, b3);
            Assert.AreEqual(0, c1);
        }

        [TestMethod]
        public void DateTimeToJsonString()
        {
            DateTime dt1 = DateTime.UtcNow;
            DateTime dt2 = Energy.Base.Clock.SetDate(dt1, 2018, 01, 01);
            string jsonString1 = Energy.Base.Cast.DateTimeToISO8601(dt2);

            Assert.AreEqual("2018-01-01T", jsonString1.Substring(0, 11));

            string displayName = "(GMT+06:00) Antarctica/Mawson Time";
            string standardName = "Mawson Time";
            TimeSpan offset = new TimeSpan(06, 00, 00);
            TimeZoneInfo mawson = TimeZoneInfo.CreateCustomTimeZone(standardName, offset, displayName, standardName);
            Debug.WriteLine("The current time is {0} {1}"
                , TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, mawson)
                , mawson.StandardName
                );
            DateTime mawsonDateTime = TimeZoneInfo.ConvertTimeFromUtc(dt2, mawson);
            string jsonStringMawson = Energy.Base.Cast.DateTimeToISO8601(mawsonDateTime);
        }

        [TestMethod]
        public void Base64ToByteArray()
        {
            string s1 = "Ąę";
            Assert.AreEqual("xITEmQ==", Energy.Base.Cast.StringToBase64(s1));
            byte[] b1 = new byte[] { 0xff, 0xff };
            string s2 = Energy.Base.Cast.ByteArrayToBase64(b1);
            Assert.AreEqual("//8=", s2);
            byte[] b2 = Energy.Base.Cast.Base64ToByteArray("//8=");
            int c1 = Energy.Base.ByteArrayBuilder.Compare(b1, b2);
            Assert.AreEqual(0, c1);
        }

        [TestMethod]
        public void As()
        {
            string _number;
            string _double;
            string _decimal;
            string _float;
            foreach (object test in new object[] { "123.456", "123,456 " })
            {
                Assert.AreEqual((byte)123, Energy.Base.Cast.As<byte>(test));
                Assert.AreEqual((sbyte)123, Energy.Base.Cast.As<sbyte>(test));
                Assert.AreEqual('1', Energy.Base.Cast.As<char>(test));
                Assert.AreEqual(123.456f, Energy.Base.Cast.As<float>(test));
                Assert.AreEqual(123.456, Energy.Base.Cast.As<double>(test));
                Assert.AreEqual(123.456m, Energy.Base.Cast.As<decimal>(test));
                Assert.AreEqual((Int16)123, Energy.Base.Cast.As<Int16>(test));
                Assert.AreEqual((UInt16)123, Energy.Base.Cast.As<UInt16>(test));
                Assert.AreEqual((Int32)123, Energy.Base.Cast.As<Int32>(test));
                Assert.AreEqual((UInt32)123, Energy.Base.Cast.As<UInt32>(test));
                Assert.AreEqual((Int64)123, Energy.Base.Cast.As<Int64>(test));
                Assert.AreEqual((UInt64)123, Energy.Base.Cast.As<UInt64>(test));
                Assert.IsTrue(Energy.Base.Cast.As<bool>(test));

                _double = test as string;
                Assert.AreEqual(123.456, Energy.Base.Cast.As<double>(_double));
                _decimal = test as string;
                Assert.AreEqual(123.456m, Energy.Base.Cast.As<decimal>(_decimal));
                _float = test as string;
                Assert.AreEqual(123.456f, Energy.Base.Cast.As<float>(_float));
            }

            // bool

            Assert.IsFalse(Energy.Base.Cast.As<bool>("NULL"));
            Assert.IsFalse(Energy.Base.Cast.As<bool>("FALSE"));
            Assert.IsFalse(Energy.Base.Cast.As<bool>("0"));

            Assert.IsTrue(Energy.Base.Cast.As<bool>("0.0"));

            // int + uint

            string _int;
            _int = int.MinValue.ToString();
            Assert.AreEqual(int.MinValue, Energy.Base.Cast.As<int>(_int));
            Assert.AreEqual((uint)0, Energy.Base.Cast.As<uint>(_int));
            _int = ((decimal)int.MinValue - 1).ToString();
            Assert.AreEqual((int)0, Energy.Base.Cast.As<int>(_int));
            Assert.AreEqual((uint)0, Energy.Base.Cast.As<uint>(_int));
            _int = int.MaxValue.ToString();
            Assert.AreEqual(int.MaxValue, Energy.Base.Cast.As<int>(_int));
            Assert.AreEqual((uint)int.MaxValue, Energy.Base.Cast.As<uint>(_int));
            _int = ((uint)int.MaxValue + 1).ToString();
            Assert.AreEqual(0, Energy.Base.Cast.As<int>(_int));
            Assert.AreEqual((uint)int.MaxValue + 1, Energy.Base.Cast.As<uint>(_int));

            // long + ulong

            string _long;
            _long = long.MinValue.ToString();
            Assert.AreEqual(long.MinValue, Energy.Base.Cast.As<long>(_long));
            Assert.AreEqual((ulong)0, Energy.Base.Cast.As<ulong>(_long));
            _long = ((decimal)long.MinValue - 1).ToString(); // test for -9223372036854775808
            Assert.AreEqual((long)0, Energy.Base.Cast.As<long>(_long));
            Assert.AreEqual((ulong)0, Energy.Base.Cast.As<ulong>(_long));
            _long = long.MaxValue.ToString();
            Assert.AreEqual(long.MaxValue, Energy.Base.Cast.As<long>(_long));
            Assert.AreEqual((ulong)long.MaxValue, Energy.Base.Cast.As<ulong>(_long));
            _long = ((ulong)long.MaxValue + 1).ToString(); // test for +9223372036854775807
            Assert.AreEqual(0, Energy.Base.Cast.As<long>(_long));
            Assert.AreEqual((ulong)long.MaxValue + 1, Energy.Base.Cast.As<ulong>(_long));

            // number string

            _number = "1";
            Assert.IsTrue(Energy.Base.Cast.As<bool>(_number));
            _number = "0";
            Assert.IsFalse(Energy.Base.Cast.As<bool>(_number));
            _number = "1";
            Assert.AreEqual("+1", Energy.Base.Cast.NumberToStringSign(_number));
            Assert.AreEqual(".1", Energy.Base.Cast.NumberToStringSign(_number, ".$"));
            _number = "-1";
            Assert.AreEqual("-1", Energy.Base.Cast.NumberToStringSign(_number));
            Assert.AreEqual("$1", Energy.Base.Cast.NumberToStringSign(_number, ".$"));
            _number = "";
            Assert.AreEqual(" 0", Energy.Base.Cast.NumberToStringSign(_number));
            Assert.AreEqual("#0", Energy.Base.Cast.NumberToStringSign(_number, ".$#"));
            _number = "0.0";
            Assert.AreEqual(" 0.0", Energy.Base.Cast.NumberToStringSign(_number));
            Assert.AreEqual(" 0.0", Energy.Base.Cast.NumberToStringSign(_number, ".$"));
            Assert.AreEqual("#0.0", Energy.Base.Cast.NumberToStringSign(_number, ".$#"));
            _number = "0,";
            Assert.AreEqual(" 0,", Energy.Base.Cast.NumberToStringSign(_number));
            Assert.AreEqual(" 0,", Energy.Base.Cast.NumberToStringSign(_number, ".$"));
            Assert.AreEqual("#0,", Energy.Base.Cast.NumberToStringSign(_number, ".$#"));
            _number = "0.";
            Assert.AreEqual(" 0.", Energy.Base.Cast.NumberToStringSign(_number));
            Assert.AreEqual(" 0.", Energy.Base.Cast.NumberToStringSign(_number, ".$"));
            Assert.AreEqual("#0.", Energy.Base.Cast.NumberToStringSign(_number, ".$#"));
            _number = "0.e+12";
            Assert.AreEqual(" 0.e+12", Energy.Base.Cast.NumberToStringSign(_number));
            Assert.AreEqual(" 0.e+12", Energy.Base.Cast.NumberToStringSign(_number, ".$"));
            Assert.AreEqual("#0.e+12", Energy.Base.Cast.NumberToStringSign(_number, ".$#"));
            _number = "0.0e+12";
            Assert.AreEqual(" 0.0e+12", Energy.Base.Cast.NumberToStringSign(_number));
            Assert.AreEqual(" 0.0e+12", Energy.Base.Cast.NumberToStringSign(_number, ".$"));
            Assert.AreEqual("#0.0e+12", Energy.Base.Cast.NumberToStringSign(_number, ".$#"));

            // double + decimal + float

            _double = double.MinValue.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(double.MinValue, Energy.Base.Cast.As<double>(_double));
            _double = double.MaxValue.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(double.MaxValue, Energy.Base.Cast.As<double>(_double));

            _decimal = decimal.MinValue.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(decimal.MinValue, Energy.Base.Cast.As<decimal>(_decimal));
            _decimal = decimal.MaxValue.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(decimal.MaxValue, Energy.Base.Cast.As<decimal>(_decimal));

            _float = "3.40282347E+38";
            Assert.AreEqual(float.MaxValue, Energy.Base.Cast.As<float>(_float));
            _float = "-3.40282347E+38";
            Assert.AreEqual(float.MinValue, Energy.Base.Cast.As<float>(_float));

            Class.TextRepresentation textRepresentation = new Class.TextRepresentation();
            textRepresentation.Text = "2001-01-01";
            DateTime resultDateTime;
            resultDateTime = Energy.Base.Cast.AsDateTime(textRepresentation);
            DateTime expectDateTime;
            expectDateTime = new DateTime(2001, 01, 01);
            Assert.AreEqual(expectDateTime, resultDateTime);
            expectDateTime = new DateTime(2002, 01, 01);
            resultDateTime = Energy.Base.Cast.AsDateTime(textRepresentation, expectDateTime, DateTime.MaxValue);
            Assert.AreEqual(expectDateTime, resultDateTime);
            expectDateTime = new DateTime(2000, 01, 01);
            resultDateTime = Energy.Base.Cast.AsDateTime(textRepresentation, DateTime.MinValue, expectDateTime);
            Assert.AreEqual(expectDateTime, resultDateTime);
        }


        [TestMethod]
        public void CastIntegerToString()
        {
            int _int;
            _int = int.MaxValue; // test for +‭2147483647‬ $7FFFFFFF
            Assert.AreEqual("2147483647", Energy.Base.Cast.IntegerToString(_int));
            Assert.AreEqual("+2147483647", Energy.Base.Cast.IntegerToStringSign(_int));
            Assert.AreEqual(".2147483647", Energy.Base.Cast.IntegerToStringSign(_int, ".$"));
            _int = int.MinValue; // test for -2147483648 $FFFFFFFF
            Assert.AreEqual("-2147483648", Energy.Base.Cast.IntegerToString(_int));
            Assert.AreEqual("$2147483648", Energy.Base.Cast.IntegerToStringSign(_int, ".$"));
        }

        [TestMethod]
        public void DoubleToString()
        {
            double _double;
            _double = -1234567890.0987654321;
            Assert.AreEqual(-1234567890.0987654, _double);
            Assert.AreEqual("-1234567890.0987654", Energy.Base.Cast.DoubleToString(_double));
            Assert.AreEqual("-1234567890.0987654000", Energy.Base.Cast.DoubleToString(_double, 10, false));
            Assert.AreEqual("-1234567890.0987654", Energy.Base.Cast.DoubleToString(_double, 10, true));
            Assert.AreEqual("-1234567890.0987654", Energy.Base.Cast.DoubleToString(_double, 11, true));
            Assert.AreEqual("-1234567890.09876540000", Energy.Base.Cast.DoubleToString(_double, 11, false));
            Assert.AreEqual("-1234567890.0987", Energy.Base.Cast.DoubleToString(_double, 4));
            _double = -1E+20;
            Assert.AreEqual("-1E+20", Energy.Base.Cast.DoubleToString(_double));
            Assert.AreEqual("-1E+20", Energy.Base.Cast.DoubleToString(_double, 0));
            Assert.AreEqual("-1.0E+20", Energy.Base.Cast.DoubleToString(_double, 1));
            _double = double.MinValue;
            Assert.AreEqual("-1.7976931348623157E+308", Energy.Base.Cast.DoubleToString(_double));
            Assert.AreEqual("-1.79769E+308", Energy.Base.Cast.DoubleToString(_double, 5, false));
            // TODO Should be rounded up
            Assert.AreEqual("-1.7976E+308", Energy.Base.Cast.DoubleToString(_double, 4, false));
            _double = -1.7976931348623157;
            Assert.AreEqual("-1.7976931348623157", Energy.Base.Cast.DoubleToString(_double));
            Assert.AreEqual("-1.79769", Energy.Base.Cast.DoubleToString(_double, 5, false));
            // TODO Should be rounded up
            Assert.AreEqual("-1.7976", Energy.Base.Cast.DoubleToString(_double, 4, false));

            byte[] bytes;
            bytes = new byte[] { 102, 102, 102, 102, 102, 166, 78, 64 };
            double d;
            d = BitConverter.ToDouble(bytes, 0);
            double e;
            e = 61.3;
            Assert.AreEqual(e, d);
            string s;
            Energy.Core.Bug.Suppress("Energy.Base.Cast.*");
            Energy.Core.Bug.Suppress("Energy.Base.Cast.*", false);
            s = Energy.Base.Cast.DoubleToString(e);
            Assert.AreNotEqual("61.299999999999997", s);
            Assert.AreEqual("61.3", s);
        }

        [TestMethod]
        public void StringToStream()
        {
            string needle = "€";
            byte[] buffer;
            using (Stream stream = Energy.Base.Cast.StringToStream(needle))
            {
                int length = (int)stream.Length;
                buffer = new byte[length];
                stream.Read(buffer, 0, length);
            }
            Assert.AreEqual(0, Energy.Base.Bit.Compare(new byte[] { 226, 130, 172 }, buffer));
            using (Stream stream = Energy.Base.Cast.StringToStream(needle, encoding: Encoding.Unicode))
            {
                int length = (int)stream.Length;
                buffer = new byte[length];
                stream.Read(buffer, 0, length);
            }
            Assert.AreEqual(0, Energy.Base.Bit.Compare(new byte[] { 172, 32 }, buffer));
        }

        [TestMethod]
        public void StreamToString()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var bb = Encoding.UTF8.GetBytes("Test String");
                ms.Write(bb, 0, bb.Length);
                ms.Seek(0, SeekOrigin.Begin);
                var ss = Energy.Base.Cast.StreamToString(ms);
                Assert.AreEqual("Test String", ss);
            }
        }

        public enum OptionsEnum
        {
            None,
            One,
            Two,
            Three,
            three,
        }

        public enum NegativeEnum
        {
            M1 = -1,
            M2 = -2,
        }

        public enum DummyEnum
        {
        }

        [TestMethod]
        public void StringToEnum()
        {
            string s;
            OptionsEnum o;
            s = null;
            o = (OptionsEnum)Energy.Base.Cast.StringToEnum(s, typeof(OptionsEnum));
            Assert.AreEqual(OptionsEnum.None, o);
            s = "";
            o = (OptionsEnum)Energy.Base.Cast.StringToEnum(s, typeof(OptionsEnum));
            Assert.AreEqual(OptionsEnum.None, o);
            s = "One";
            o = (OptionsEnum)Energy.Base.Cast.StringToEnum(s, typeof(OptionsEnum));
            Assert.AreEqual(OptionsEnum.One, o);
            s = "one";
            o = (OptionsEnum)Energy.Base.Cast.StringToEnum(s, typeof(OptionsEnum));
            Assert.AreEqual(OptionsEnum.One, o);
            s = " three ";
            o = (OptionsEnum)Energy.Base.Cast.StringToEnum(s, typeof(OptionsEnum));
            Assert.AreEqual(OptionsEnum.Three, o);
            s = " three ";
            o = (OptionsEnum)Energy.Base.Cast.StringToEnum(s, typeof(OptionsEnum), false);
            Assert.AreEqual(OptionsEnum.three, o);
            s = " three ";
            o = Energy.Base.Cast.StringToEnum<OptionsEnum>(s);
            Assert.AreEqual(OptionsEnum.Three, o);
            s = " three ";
            o = Energy.Base.Cast.StringToEnum<OptionsEnum>(s, true);
            Assert.AreEqual(OptionsEnum.Three, o);
            s = " three ";
            o = Energy.Base.Cast.StringToEnum<OptionsEnum>(s, false);
            Assert.AreEqual(OptionsEnum.three, o);
        }

        [Flags]
        public enum BitsEnum
        {
            B0 = 0,
            B1 = 1,
            B2 = 2,
            B3 = 4,
        }

        [TestMethod]
        public void IntToEnum()
        {
            int i;
            BitsEnum b;

            i = 0;
            b = Energy.Base.Cast.IntToEnum<BitsEnum>(i);
            Assert.AreEqual(BitsEnum.B0, b);
            i = 1;
            b = Energy.Base.Cast.IntToEnum<BitsEnum>(i);
            Assert.AreEqual(BitsEnum.B1, b);
            i = 2;
            b = Energy.Base.Cast.IntToEnum<BitsEnum>(i);
            Assert.AreEqual(BitsEnum.B2, b);
            i = 3;
            b = Energy.Base.Cast.IntToEnum<BitsEnum>(i);
            Assert.AreEqual(BitsEnum.B1 | BitsEnum.B2, b);
            i = 4;
            b = Energy.Base.Cast.IntToEnum<BitsEnum>(i);
            Assert.AreEqual(BitsEnum.B3, b);
            i = 5;
            b = Energy.Base.Cast.IntToEnum<BitsEnum>(i);
            Assert.AreEqual(BitsEnum.B1 | BitsEnum.B3, b);

            i = -1;
            b = Energy.Base.Cast.IntToEnum<BitsEnum>(i);
            Assert.AreEqual(-1, (int)b);
        }

        [TestMethod]
        public void DateTimeToString()
        {
            DateTime value;
            string result;
            string expect;

            value = DateTime.MinValue;
            expect = "";
            result = Energy.Base.Cast.DateTimeToString(value, null, null, "", null);
            Assert.AreEqual(expect, result);

            value = new DateTime(1753, 1, 1);
            expect = "1753-01-01";
            result = Energy.Base.Cast.DateTimeToString(value, null, null, "", null);
            Assert.AreEqual(expect, result);

            value = new DateTime(1753, 1, 1).AddMilliseconds(1);
            expect = "1753-01-01 00:00:00.001";
            result = Energy.Base.Cast.DateTimeToString(value, null, null, "", null);
            Assert.AreEqual(expect, result);

            value = new DateTime(1753, 1, 1);
            expect = "1753-01-01";
            result = Energy.Base.Cast.DateTimeToString(value, "yyyy-MM-dd", null, "", null);
            Assert.AreEqual(expect, result);

            value = new DateTime(1753, 1, 1);
            expect = "N/A";
            result = Energy.Base.Cast.DateTimeToString(value, "yyyy-MM-dd", null, "N/A", new DateTime[] { new DateTime(1753, 1, 1) });
            Assert.AreEqual(expect, result);

            value = new DateTime(1753, 1, 1);
            result = Energy.Base.Cast.DateTimeToString(value, "", null, null, new DateTime[] { new DateTime(1753, 1, 1) });
            Assert.IsNull(result);

            value = DateTime.MinValue;
            expect = "";
            result = Energy.Base.Cast.DateTimeToString(value, "", null, null, new DateTime[] { new DateTime(1753, 1, 1) });
            Assert.AreEqual(expect, result);

            value = DateTime.MinValue;
            expect = "0001-01-01";
            result = Energy.Base.Cast.DateTimeToString(value, "yyyy-MM-dd", null, null, new DateTime[] { new DateTime(1753, 1, 1) });
            Assert.AreEqual(expect, result);
        }

        [TestMethod]
        public void StringToShort()
        {
            short expect;
            short result;
            string needle;

            needle = "1234567";
            result = Energy.Base.Cast.StringToShort(needle, false, false);
            expect = 0;
            Assert.AreEqual(expect, result);
            result = Energy.Base.Cast.StringToShort(needle, false, true);
            expect = 22151;
            Assert.AreEqual(expect, result);

            needle = " \t1234567.9999999 \r\n ";
            result = Energy.Base.Cast.StringToShort(needle, false, false);
            expect = 0;
            Assert.AreEqual(expect, result);
            result = Energy.Base.Cast.StringToShort(needle, true, false);
            expect = 0;
            Assert.AreEqual(expect, result);
            result = Energy.Base.Cast.StringToShort(needle, false, true);
            expect = 0;
            Assert.AreEqual(expect, result);
            result = Energy.Base.Cast.StringToShort(needle, true, true);
            expect = 22151;
            Assert.AreEqual(expect, result);
        }

        [TestMethod]
        public void StringToUnsignedShort()
        {
            ushort expect;
            ushort result;
            string needle;

            needle = "1234567";
            result = Energy.Base.Cast.StringToUnsignedShort(needle, false, false);
            expect = 0;
            Assert.AreEqual(expect, result);
            result = Energy.Base.Cast.StringToUnsignedShort(needle, false, true);
            expect = 54919;
            Assert.AreEqual(expect, result);

            needle = " \t1234567.9999999 \r\n ";
            result = Energy.Base.Cast.StringToUnsignedShort(needle, false, false);
            expect = 0;
            Assert.AreEqual(expect, result);
            result = Energy.Base.Cast.StringToUnsignedShort(needle, true, false);
            expect = 0;
            Assert.AreEqual(expect, result);
            result = Energy.Base.Cast.StringToUnsignedShort(needle, false, true);
            expect = 0;
            Assert.AreEqual(expect, result);
            result = Energy.Base.Cast.StringToUnsignedShort(needle, true, true);
            expect = 54919;
            Assert.AreEqual(expect, result);

            needle = "-67";
            result = Energy.Base.Cast.StringToUnsignedShort(needle, false, false);
            expect = 0;
            Assert.AreEqual(expect, result);
            result = Energy.Base.Cast.StringToUnsignedShort(needle, false, true);
            expect = 67;
            Assert.AreEqual(expect, result);
        }
        
        [TestMethod]
        public void StringToInteger()
        {
            string s;
            int i;

            s = "1000";
            i = Energy.Base.Cast.StringToInteger(s);
            Assert.AreEqual(1000, i);

            s = " \t\n1000\r\v";
            i = Energy.Base.Cast.StringToInteger(s);
            Assert.AreEqual(1000, i);
        }

        [TestMethod]
        public void StringToInt()
        {
            string s;
            int i;

            s = "1000";
            i = Energy.Base.Cast.StringToInt(s);
            Assert.AreEqual(1000, i);

            s = " \t\n1000\r\v";
            i = Energy.Base.Cast.StringToInt(s);
            Assert.AreEqual(1000, i);

            int n = Energy.Core.Benchmark.Loop(() => i = Energy.Base.Cast.StringToInt(s), 1);
            Debug.WriteLine(n);
            Assert.AreNotEqual(0, n);
        }
    }
}
