using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Hash
    {
        [TestMethod]
        public void HashMD5()
        {
            string text, hash, must;
            text = "How are you?";
            hash = Energy.Base.Hash.MD5(text);
            must = "04e35eb3e4fcb8b395191053c359ca0e";
            Assert.AreEqual(must, hash);
            text = "Ąę";
            hash = Energy.Base.Hash.MD5(text);
            must = "ee0fa1428266181ce99c32fc5394a507";
            Assert.AreEqual(must, hash);
        }

        [TestMethod]
        public void HashSHA1()
        {
            string text, hash, must;
            text = "How are you?";
            hash = Energy.Base.Hash.SHA1(text);
            must = "3031897e282167593fbb4dbe81dc48ebbe9a002d";
            Assert.AreEqual(must, hash);
            text = "Ąę";
            hash = Energy.Base.Hash.SHA1(text);
            must = "4fa671507db9af62afc8c0e9b685df6d5d85cf32";
            Assert.AreEqual(must, hash);
        }

        [TestMethod]
        public void HashSHA256()
        {
            string text, hash, must;
            text = "How are you?";
            hash = Energy.Base.Hash.SHA256(text);
            must = "df287dfc1406ed2b692e1c2c783bb5cec97eac53151ee1d9810397aa0afa0d89";
            Assert.AreEqual(must, hash);
            text = "Ąę";
            hash = Energy.Base.Hash.SHA256(text);
            must = "e5287d6cf15960f2013178f1a3438614837798b6ec0e42f0403886019400398b";
            Assert.AreEqual(must, hash);
        }
    }
}
