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

        [TestMethod]
        public void HashSHA384()
        {
            string text, hash, must;
            text = "How are you?";
            hash = Energy.Base.Hash.SHA384(text);
            must = "964958303aa7fb4bd7028909df59f5120deec0390ec2b73522e75244920f0d94a30c89d1da457ae97e832a2fcbf02369";
            Assert.AreEqual(must, hash);
            text = "Ąę";
            hash = Energy.Base.Hash.SHA384(text);
            must = "3488f732829d67d9539aecb72c310c8d47233c436d4a7bc15ca8f246eb072666f342c75b74b14fc4c26545d864bf8d70";
            Assert.AreEqual(must, hash);
        }

        [TestMethod]
        public void HashSHA512()
        {
            string text, hash, must;
            text = "How are you?";
            hash = Energy.Base.Hash.SHA512(text);
            must = "B4664D8CDD2A11410ED1EF1E5C0889B3E0B8CF8DD603F92AFE87DAE8E3F13C66C9791DC7DC6AB68CD81998C85BBE64BDB54917E0E025AF53A65649D1FFB5DE31";
            Assert.IsTrue(0 == string.Compare(must, hash, true));
            text = "Ąę";
            hash = Energy.Base.Hash.SHA512(text);
            must = "70E06D64116978A3C4615635C1EBE149822AF49602F897C960C80E91D8819E0A94B3F357AA776488B28A02550B71D6397D5B124AC9AE7762471BD5B2104142E9";
            Assert.IsTrue(0 == string.Compare(must, hash, true));
        }
    }
}
