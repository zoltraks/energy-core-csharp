using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Sheet
    {
        [TestMethod]
        public void Address()
        {
            {
                var a1 = Energy.Base.Sheet.Address.From("AAA111111111111111111");
                Assert.AreEqual("AAA", a1.Column);
#pragma warning disable CS0078 // The 'l' suffix is easily confused with the digit '1'
                Assert.AreEqual(111111111111111111l, a1.Row);
#pragma warning restore CS0078 // The 'l' suffix is easily confused with the digit '1'
                Assert.IsFalse(a1.RowLess);
                Assert.IsFalse(a1.ColumnLess);
                Assert.IsFalse(a1.StickyRow);
                Assert.IsFalse(a1.StickyColumn);
            }

            {
                var a2 = Energy.Base.Sheet.Address.From("AAA");
                Assert.AreEqual("AAA", a2.Column);
                Assert.AreEqual(0, a2.Row);
                Assert.IsTrue(a2.RowLess);
                Assert.IsFalse(a2.ColumnLess);
                Assert.IsFalse(a2.StickyRow);
                Assert.IsFalse(a2.StickyColumn);
            }

            {
                var a3 = Energy.Base.Sheet.Address.From("$A$1");
                Assert.AreEqual("A", a3.Column);
                Assert.AreEqual(1, a3.Row);
                Assert.IsFalse(a3.RowLess);
                Assert.IsFalse(a3.ColumnLess);
                Assert.IsTrue(a3.StickyRow);
                Assert.IsTrue(a3.StickyColumn);
            }

            {
                var a4 = Energy.Base.Sheet.Address.From("$9");
                Assert.AreEqual("", a4.Column);
                Assert.AreEqual(9, a4.Row);
                Assert.IsFalse(a4.RowLess);
                Assert.IsTrue(a4.ColumnLess);
                Assert.IsTrue(a4.StickyRow);
                Assert.IsFalse(a4.StickyColumn);
            }
        }

        [TestMethod]
        public void Index()
        {
            long n;
            n = Energy.Base.Sheet.Address.GetColumnIndex("A");
            Assert.AreEqual(1, n);
            n = Energy.Base.Sheet.Address.GetColumnIndex("Z");
            Assert.AreEqual(26, n);
            n = Energy.Base.Sheet.Address.GetColumnIndex("AA");
            Assert.AreEqual(27, n);
            n = Energy.Base.Sheet.Address.GetColumnIndex("AB");
            Assert.AreEqual(28, n);
            n = Energy.Base.Sheet.Address.GetColumnIndex("");
            Assert.AreEqual(0, n);
            n = Energy.Base.Sheet.Address.GetColumnIndex("1");
            Assert.AreEqual(0, n);
            n = Energy.Base.Sheet.Address.GetColumnIndex("-1");
            Assert.AreEqual(0, n);
        }

        [TestMethod]
        public void Range()
        {
            {
                var a = Energy.Base.Sheet.Range.From("AAA111111111111111111");
                Assert.AreEqual("AAA", a.Begin.Column);
                Assert.AreEqual(111111111111111111, a.Begin.Row);
                Assert.IsFalse(a.Begin.RowLess);
                Assert.IsFalse(a.Begin.ColumnLess);
                Assert.IsFalse(a.Begin.StickyRow);
                Assert.IsFalse(a.Begin.StickyColumn);
                Assert.IsTrue(a.To.RowLess);
                Assert.IsTrue(a.To.ColumnLess);
                Assert.IsFalse(a.To.StickyRow);
                Assert.IsFalse(a.To.StickyColumn);
            }

            {
                var a = Energy.Base.Sheet.Range.From("A1:B2");
                Assert.AreEqual("A", a.Begin.Column);
                Assert.AreEqual("A1", a.Begin.ToString());
                Assert.AreEqual("B", a.To.Column);
                Assert.AreEqual("B2", a.To.ToString());
                Assert.IsFalse(a.Begin.RowLess);
                Assert.IsFalse(a.Begin.ColumnLess);
                Assert.IsFalse(a.Begin.StickyRow);
                Assert.IsFalse(a.Begin.StickyColumn);
                Assert.IsFalse(a.To.RowLess);
                Assert.IsFalse(a.To.ColumnLess);
                Assert.IsFalse(a.To.StickyRow);
                Assert.IsFalse(a.To.StickyColumn);
            }
        }

    }
}

