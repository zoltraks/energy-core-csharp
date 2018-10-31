using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Cast
    {
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
        public void CastStringToInteger()
        {
            string str1 = "15,001";
            int num1 = Energy.Base.Cast.StringToInteger(str1);
            Assert.AreEqual(15, num1);
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
        }

        private class ExampleTypeStructure
        {
            private Int64? _NInt64;
            public Int64? NInt64 { get { return _NInt64; } set { _NInt64 = value; } }
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
            int c1 = Energy.Base.Byte.Compare(b2, b3);
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
            object test = "123.456";
            Assert.AreEqual((byte)123, Energy.Base.Cast.As<byte>(test));
            Assert.AreEqual((sbyte)123, Energy.Base.Cast.As<sbyte>(test));
            Assert.AreEqual('1', Energy.Base.Cast.As<char>(test));
        }
    }
}
