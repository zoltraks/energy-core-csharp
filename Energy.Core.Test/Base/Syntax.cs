using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Syntax
    {
        [TestMethod]
        public void SyntaxParseDefault()
        {
            Energy.Core.Syntax syntax = Energy.Core.Syntax.Default;
            syntax.OnReset += Syntax_OnReset;
            syntax.Reset();
            string value = syntax.Parse("{{A}}-{{B}}");
            Assert.AreEqual("-", value);
        }

        private void Syntax_OnReset(Energy.Core.Syntax syntax)
        {
            syntax["A"] = "1";
            syntax["B"] = "2";
        }
    }
}
