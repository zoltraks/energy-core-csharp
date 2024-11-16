using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Core
{
    [TestClass]
    public class Editor
    {
        [TestMethod]
        public void EditorEnsureNewLineAtEnd()
        {
            string needle, result, expect;
            needle = "";
            result = Energy.Core.Editor.Global.EnsureNewLineAtEnd(needle);
            expect = "";
            Assert.AreEqual(expect, result);
            needle = "-";
            result = Energy.Core.Editor.Global.EnsureNewLineAtEnd(needle);
            expect = "-" + Energy.Base.Text.NewLine;
            Assert.AreEqual(expect, result);
        }
    }
}
