using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Number
    {
        [TestMethod]
        public void Median()
        {
            int[] _int;
            long[] _long;
            ulong[] _ulong;
            double[] _double;
            decimal[] _decimal;

            _int = new int[] { 1, 2, 3 };
            _long = new long[] { 1, 2, 3 };
            _ulong = new ulong[] { 1, 2, 3 };
            _double = new double[] { 1, 2, 3 };
            _decimal = new decimal[] { 1, 2, 3 };

            Assert.AreEqual(2, Energy.Base.Number.Median(_int));
            Assert.AreEqual(2, Energy.Base.Number.Median(_long));
            Assert.AreEqual((ulong)2, Energy.Base.Number.Median(_ulong));
            Assert.AreEqual(2, Energy.Base.Number.Median(_double));
            Assert.AreEqual(2, Energy.Base.Number.Median(_decimal));

            _int = new int[] { 1, 2, 3, 4 };
            _long = new long[] { 1, 2, 3, 4 };
            _ulong = new ulong[] { 1, 2, 3, 4 };
            _double = new double[] { 1, 2, 3, 4 };
            _decimal = new decimal[] { 1, 2, 3, 4 };

            Assert.AreEqual(2, Energy.Base.Number.Median(_int));
            Assert.AreEqual(2, Energy.Base.Number.Median(_long));
            Assert.AreEqual((ulong)2, Energy.Base.Number.Median(_ulong));
            Assert.AreEqual(2.5, Energy.Base.Number.Median(_double));
            Assert.AreEqual(2.5m, Energy.Base.Number.Median(_decimal));
        }
    }
}
