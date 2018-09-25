using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Class
    {
        [TestMethod]
        public void ClassGetDefault()
        {
            object result;
            object expect;
            result = Energy.Base.Class.GetDefault(typeof(DateTime));
            expect = DateTime.MinValue;
            Assert.AreEqual(expect, result);
        }
    }
}
