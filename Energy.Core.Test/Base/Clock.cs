using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Clock
    {
        [TestMethod]
        public void HasExpired()
        {
            DateTime? d1;
            Assert.IsFalse(Energy.Base.Clock.HasExpired(null, -1, out d1, null));
            Assert.IsNull(d1);
            Assert.IsFalse(Energy.Base.Clock.HasExpired(null, -1, out d1));
            Assert.IsFalse(Energy.Base.Clock.HasExpired(null, -1));
            Assert.IsTrue(Energy.Base.Clock.HasExpired(null, 0));
            Assert.IsTrue(Energy.Base.Clock.HasExpired(d1, 0.00001, out d1));
            Assert.IsNotNull(d1);
        }

        [TestMethod]
        public void HasFractionalPart()
        {
            DateTime d;
            d = DateTime.MinValue;
            Assert.IsFalse(Energy.Base.Clock.HasFractionalPart(d));
            d = d.AddTicks(1);
            Assert.IsTrue(Energy.Base.Clock.HasFractionalPart(d));
            d = d.AddTicks(TimeSpan.TicksPerSecond - 2);
            Assert.IsTrue(Energy.Base.Clock.HasFractionalPart(d));
            d = d.AddTicks(1);
            Assert.IsFalse(Energy.Base.Clock.HasFractionalPart(d));
        }

        [TestMethod]
        public void GetZoneString()
        {
            DateTime d;
            string s;
            d = DateTime.MinValue; //.ToUniversalTime();
            s = Energy.Base.Clock.GetZoneString(d);
            Assert.AreEqual("", s);
            d = d.AddTicks(1);
            s = Energy.Base.Clock.GetZoneString(d);
            Assert.IsTrue(s.StartsWith("00:00:00.000 "));
            s = Energy.Base.Clock.GetZoneString(d, 6);
            Assert.IsTrue(s.StartsWith("00:00:00.000000 "));
            s = Energy.Base.Clock.GetZoneString(d, 7);
            Assert.IsTrue(s.StartsWith("00:00:00.0000001 "));
            s = Energy.Base.Clock.GetZoneString(d, 8);
            Assert.IsTrue(s.StartsWith("00:00:00.0000001 "));
            d = d.AddDays(1);
            d = d.AddTicks(TimeSpan.TicksPerSecond - 1);
            s = Energy.Base.Clock.GetZoneString(d);
            Assert.IsTrue(s.StartsWith("0001-01-02 00:00:01 "));
            s = Energy.Base.Clock.GetZoneString(d, 3);
            Assert.IsTrue(s.StartsWith("0001-01-02 00:00:01 "));
            s = Energy.Base.Clock.GetZoneString(d, -3);
            Assert.IsTrue(s.StartsWith("0001-01-02 00:00:01.000 "));
        }

        [TestMethod]
        public void Floor()
        {
            DateTime needle;
            DateTime result;
            DateTime expect;
            needle = new DateTime(2121, 12, 16, 21, 17, 33, 456);
            expect = new DateTime(2121, 12, 16, 21, 17, 33, 456);
            result = Energy.Base.Clock.Floor(needle, 4);
            Assert.AreEqual(expect, result);
            result = Energy.Base.Clock.Floor(needle, 3);
            Assert.AreEqual(expect, result);
            expect = new DateTime(2121, 12, 16, 21, 17, 33, 450);
            result = Energy.Base.Clock.Floor(needle, 2);
            Assert.AreEqual(expect, result);
            expect = new DateTime(2121, 12, 16, 21, 17, 33, 400);
            result = Energy.Base.Clock.Floor(needle, 1);
            Assert.AreEqual(expect, result);
            expect = new DateTime(2121, 12, 16, 21, 17, 33, 0);
            result = Energy.Base.Clock.Floor(needle, 0);
            Assert.AreEqual(expect, result);
            expect = new DateTime(2121, 12, 16, 21, 17, 30, 0);
            result = Energy.Base.Clock.Floor(needle, -1);
            Assert.AreEqual(expect, result);
            expect = new DateTime(2121, 12, 16, 21, 17, 0, 0);
            result = Energy.Base.Clock.Floor(needle, -2);
            Assert.AreEqual(expect, result);
            expect = new DateTime(2121, 12, 16, 21, 10, 0, 0);
            result = Energy.Base.Clock.Floor(needle, -3);
            Assert.AreEqual(expect, result);
            expect = new DateTime(2121, 12, 16, 21, 0, 0, 0);
            result = Energy.Base.Clock.Floor(needle, -4);
            Assert.AreEqual(expect, result);
            expect = new DateTime(2121, 12, 16, 20, 0, 0, 0);
            result = Energy.Base.Clock.Floor(needle, -5);
            Assert.AreEqual(expect, result);
            expect = new DateTime(2121, 12, 16, 0, 0, 0, 0);
            result = Energy.Base.Clock.Floor(needle, -6);
            Assert.AreEqual(expect, result);
            expect = new DateTime(2121, 12, 10, 0, 0, 0, 0);
            result = Energy.Base.Clock.Floor(needle, -7);
            Assert.AreEqual(expect, result);
            expect = new DateTime(2121, 12, 1, 0, 0, 0, 0);
            result = Energy.Base.Clock.Floor(needle, -8);
            Assert.AreEqual(expect, result);
            expect = new DateTime(2121, 10, 1, 0, 0, 0, 0);
            result = Energy.Base.Clock.Floor(needle, -9);
            Assert.AreEqual(expect, result);
            expect = new DateTime(2121, 1, 1, 0, 0, 0, 0);
            result = Energy.Base.Clock.Floor(needle, -10);
            Assert.AreEqual(expect, result);
            expect = new DateTime(2120, 1, 1, 0, 0, 0, 0);
            result = Energy.Base.Clock.Floor(needle, -11);
            Assert.AreEqual(expect, result);
            expect = new DateTime(2100, 1, 1, 0, 0, 0, 0);
            result = Energy.Base.Clock.Floor(needle, -12);
            Assert.AreEqual(expect, result);
            expect = new DateTime(2000, 1, 1, 0, 0, 0, 0);
            result = Energy.Base.Clock.Floor(needle, -13);
            Assert.AreEqual(expect, result);
            expect = DateTime.MinValue;
            result = Energy.Base.Clock.Floor(needle, -14);
            Assert.AreEqual(expect, result);
            result = Energy.Base.Clock.Floor(needle, -15);
            Assert.AreEqual(expect, result);
        }
    }
}
