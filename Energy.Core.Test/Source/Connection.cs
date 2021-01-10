using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Energy.Core.Test.Source
{
    [TestClass]
    public class Connection
    {
        [TestMethod]
        public void EmptyQuery()
        {
            var db = new Energy.Source.Connection();
            string error;
            object result;
            int number;
            DataTable table;
            Energy.Base.Table fetch;
            result = db.Scalar(null, out error);
            Assert.AreEqual("Empty query", error);
            Assert.IsNull(result);
            result = db.Scalar("", out error);
            Assert.AreEqual("Empty query", error);
            Assert.IsNull(result);
            number = db.Scalar<int>(null, out error);
            Assert.AreEqual("Empty query", error);
            Assert.AreEqual(0, number);
            number = db.Execute(null, out error);
            Assert.AreEqual("Empty query", error);
            Assert.AreEqual(-2, number);
            table = db.Read(null, out error);
            Assert.AreEqual("Empty query", error);
            Assert.IsNull(table);
            fetch = db.Fetch(null, out error);
            Assert.AreEqual("Empty query", error);
            Assert.IsNull(fetch);
            table = db.Load(null, out error);
            Assert.AreEqual("Empty query", error);
            Assert.IsNull(table);
        }
    }
}
