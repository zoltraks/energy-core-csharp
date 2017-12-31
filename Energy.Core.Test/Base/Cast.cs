using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Cast
    {
        [TestMethod]
        public void StringToDouble()
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
    }
}
