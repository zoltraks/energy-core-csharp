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
            string str2 = " 15.000000001 ";
            double num2 = Energy.Base.Cast.StringToDouble(str2);
            Assert.AreEqual(15.000000001, num2);
            string str3 = " -1,234 ";
            double num3 = Energy.Base.Cast.StringToDouble(str3);
            Assert.AreEqual(-1.234, num3);
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
            byte[] b1 = new byte[] { 0xfe , 0xff, 0xff, 0xf0, 0x90, 0xbc };
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
    }
}
