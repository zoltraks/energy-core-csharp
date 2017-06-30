using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Color
    {
        [TestMethod]
        public void ColorToString()
        {
            Energy.Base.Color c1 = "#333";
            string s1 = (string)c1;
            Debug.WriteLine(s1);
            Assert.AreEqual("#333333", s1);
            Energy.Base.Color c2 = "#ccc";
            string s2 = (string)c2;
            Debug.WriteLine(s2);
            Assert.AreEqual("#cccccc", s2);
            Energy.Base.Color c3 = Energy.Base.Color.ColorTransition(c1, c2, 0.5);
            string s3 = (string)c3;
            Debug.WriteLine(s3);
            Assert.AreEqual("#7f7f7f", s3);
        }
    }
}
