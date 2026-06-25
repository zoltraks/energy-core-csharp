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
            Assert.HasCount(0, r);

            r = Energy.Base.Expression.MatchNamedGroups("a b c", null, RegexOptions.None, new string[] { });
            Assert.IsNotNull(r);
            Assert.HasCount(0, r);

            r = Energy.Base.Expression.MatchNamedGroups("a b c", null, RegexOptions.None, new string[] { "X" });
            Assert.IsNotNull(r);
            Assert.HasCount(1, r);
            Assert.IsNull(r["X"]);

            r = Energy.Base.Expression.MatchNamedGroups("a b c", null, RegexOptions.None, new string[] { "X", "Y" });
            Assert.IsNotNull(r);
            Assert.HasCount(2, r);
            Assert.IsNull(r["X"]);
            Assert.IsNull(r["Y"]);

            r = Energy.Base.Expression.MatchNamedGroups("a b c", @"(?<X>\w+)", RegexOptions.None, new string[] { "X", "Y" });
            Assert.IsNotNull(r);
            Assert.HasCount(2, r);
            Assert.AreEqual("a", r["X"]);
            Assert.IsNull(r["Y"]);

            r = Energy.Base.Expression.MatchNamedGroups("ab123", @"(?<X>\p{L}+)(?<Y>\d+)", RegexOptions.None, null);
            Assert.IsNotNull(r);
            Assert.HasCount(2, r);
            Assert.AreEqual("ab", r["X"]);
            Assert.AreEqual("123", r["Y"]);
        }
    }
}