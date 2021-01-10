using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Expression
    {
        [TestMethod]
        public void MatchNamedGroups()
        {
            Dictionary<string, string> r;

            r = Energy.Base.Expression.MatchNamedGroups("a b c", null, RegexOptions.None, null);
            Assert.IsNotNull(r);
            Assert.AreEqual(0, r.Count);

            r = Energy.Base.Expression.MatchNamedGroups("a b c", null, RegexOptions.None, new string[] { });
            Assert.IsNotNull(r);
            Assert.AreEqual(0, r.Count);

            r = Energy.Base.Expression.MatchNamedGroups("a b c", null, RegexOptions.None, new string[] { "X" });
            Assert.IsNotNull(r);
            Assert.AreEqual(1, r.Count);
            Assert.AreEqual(null, r["X"]);

            r = Energy.Base.Expression.MatchNamedGroups("a b c", null, RegexOptions.None, new string[] { "X", "Y" });
            Assert.IsNotNull(r);
            Assert.AreEqual(2, r.Count);
            Assert.AreEqual(null, r["X"]);
            Assert.AreEqual(null, r["Y"]);

            r = Energy.Base.Expression.MatchNamedGroups("a b c", @"(?<X>\w+)", RegexOptions.None, new string[] { "X", "Y" });
            Assert.IsNotNull(r);
            Assert.AreEqual(2, r.Count);
            Assert.AreEqual("a", r["X"]);
            Assert.AreEqual(null, r["Y"]);

            r = Energy.Base.Expression.MatchNamedGroups("ab123", @"(?<X>\p{L}+)(?<Y>\d+)", RegexOptions.None, null);
            Assert.IsNotNull(r);
            Assert.AreEqual(2, r.Count);
            Assert.AreEqual("ab", r["X"]);
            Assert.AreEqual("123", r["Y"]);
        }
    }
}