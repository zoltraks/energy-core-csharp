using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Bracket
    {
        [TestMethod]
        public void Bracket_DefaultConstructor()
        {
            var bracket = new Energy.Base.Bracket();
            Assert.AreEqual("", bracket.Enclosure);
            Assert.AreEqual("", bracket.Prefix);
            Assert.AreEqual("", bracket.Suffix);
        }

        [TestMethod]
        public void Bracket_EnclosureConstructor()
        {
            var bracket = new Energy.Base.Bracket("[]");
            Assert.AreEqual("[]", bracket.Enclosure);
            Assert.AreEqual("[", bracket.Prefix);
            Assert.AreEqual("]", bracket.Suffix);
        }

        [TestMethod]
        public void Bracket_PrefixSuffixConstructor()
        {
            var bracket = new Energy.Base.Bracket("{{", "}}");
            Assert.AreEqual("{{", bracket.Prefix);
            Assert.AreEqual("}}", bracket.Suffix);
            Assert.AreEqual("{{}}", bracket.Enclosure);
        }

        [TestMethod]
        public void Bracket_FullConstructor()
        {
            var bracket = new Energy.Base.Bracket("/*", "*/", "comment");
            Assert.AreEqual("/*", bracket.Prefix);
            Assert.AreEqual("*/", bracket.Suffix);
            Assert.AreEqual("/**/", bracket.Enclosure);
        }

        [TestMethod]
        public void Bracket_CommonBrackets()
        {
            // Test common bracket types
            var curly = new Energy.Base.Bracket("{}");
            Assert.AreEqual("{", curly.Prefix);
            Assert.AreEqual("}", curly.Suffix);

            var square = new Energy.Base.Bracket("[]");
            Assert.AreEqual("[", square.Prefix);
            Assert.AreEqual("]", square.Suffix);

            var round = new Energy.Base.Bracket("()");
            Assert.AreEqual("(", round.Prefix);
            Assert.AreEqual(")", round.Suffix);

            var angle = new Energy.Base.Bracket("<>");
            Assert.AreEqual("<", angle.Prefix);
            Assert.AreEqual(">", angle.Suffix);
        }

        [TestMethod]
        public void Bracket_SpecialBrackets()
        {
            // Test special bracket types
            var doubleCurly = new Energy.Base.Bracket("{{}}");
            Assert.AreEqual("{{", doubleCurly.Prefix);
            Assert.AreEqual("}}", doubleCurly.Suffix);

            var quote = new Energy.Base.Bracket("\"\"");
            Assert.AreEqual("\"", quote.Prefix);
            Assert.AreEqual("\"", quote.Suffix);

            var singleQuote = new Energy.Base.Bracket("''");
            Assert.AreEqual("'", singleQuote.Prefix);
            Assert.AreEqual("'", singleQuote.Suffix);
        }

        [TestMethod]
        public void Bracket_Array_GetMatchExpression()
        {
            var array = new Energy.Base.Bracket.Array();
            
            // Empty array
            Assert.AreEqual("", array.GetMatchExpression());

            // Single bracket
            array.Add(new Energy.Base.Bracket("{}"));
            Assert.AreEqual("\\{([^}]*)}", array.GetMatchExpression());

            // Multiple brackets
            array.Add(new Energy.Base.Bracket("[]"));
            array.Add(new Energy.Base.Bracket("()"));
            string expression = array.GetMatchExpression();
            Assert.IsTrue(expression.Contains(@"\{([^}]*)}"));
            Assert.IsTrue(expression.Contains(@"\[([^]]*)]"));
            Assert.IsTrue(expression.Contains(@"\(([^\)]*)\)"));
            Assert.IsTrue(expression.Contains("|"));
        }

        [TestMethod]
        public void Bracket_CustomBrackets()
        {
            // Test custom bracket configurations
            var xml = new Energy.Base.Bracket("<", ">");
            Assert.AreEqual("<", xml.Prefix);
            Assert.AreEqual(">", xml.Suffix);

            var htmlComment = new Energy.Base.Bracket("<!--", "-->");
            Assert.AreEqual("<!--", htmlComment.Prefix);
            Assert.AreEqual("-->", htmlComment.Suffix);

            var cComment = new Energy.Base.Bracket("/*", "*/");
            Assert.AreEqual("/*", cComment.Prefix);
            Assert.AreEqual("*/", cComment.Suffix);
        }

        [TestMethod]
        public void Bracket_Equality()
        {
            var bracket1 = new Energy.Base.Bracket("{}");
            var bracket2 = new Energy.Base.Bracket("{}");
            var bracket3 = new Energy.Base.Bracket("[]");

            // Test reference equality (different instances)
            Assert.AreNotSame(bracket1, bracket2);
            Assert.AreNotSame(bracket1, bracket3);

            // Test value equality through properties
            Assert.AreEqual(bracket1.Prefix, bracket2.Prefix);
            Assert.AreEqual(bracket1.Suffix, bracket2.Suffix);
            Assert.AreNotEqual(bracket1.Prefix, bracket3.Prefix);
        }

        [TestMethod]
        public void Bracket_ArrayOperations()
        {
            var array = new Energy.Base.Bracket.Array();
            
            // Add brackets
            array.Add(new Energy.Base.Bracket("{}"));
            array.Add(new Energy.Base.Bracket("[]"));
            array.Add(new Energy.Base.Bracket("()"));

            Assert.AreEqual(3, array.Count);
            // Check if bracket with same enclosure exists
            var foundBracket = array.Find(b => b.Enclosure == "{}");
            Assert.IsNotNull(foundBracket);

            // Remove bracket
            array.RemoveAt(1); // Remove the second bracket ([])
            Assert.AreEqual(2, array.Count);

            // Clear array
            array.Clear();
            Assert.AreEqual(0, array.Count);
        }

        [TestMethod]
        public void Bracket_EdgeCases()
        {
            // Test empty strings
            var emptyBracket = new Energy.Base.Bracket("", "");
            Assert.AreEqual("", emptyBracket.Prefix);
            Assert.AreEqual("", emptyBracket.Suffix);

            // Test single character brackets
            var singleChar = new Energy.Base.Bracket("a", "b");
            Assert.AreEqual("a", singleChar.Prefix);
            Assert.AreEqual("b", singleChar.Suffix);

            // Test multi-character brackets
            var multiChar = new Energy.Base.Bracket("<<<", ">>>");
            Assert.AreEqual("<<<", multiChar.Prefix);
            Assert.AreEqual(">>>", multiChar.Suffix);
        }
    }
}
