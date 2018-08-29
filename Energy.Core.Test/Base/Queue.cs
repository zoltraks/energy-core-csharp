﻿using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Base
{
    [TestClass]
    public class Queue
    {
        [TestMethod]
        public void QueueLimit()
        {
            Energy.Base.Queue<string> q = new Energy.Base.Queue<string>();
            q.Limit = 2;
            q.Push("ABC");
            q.Push("DEF");
            Assert.IsFalse(q.Push("GHI"), "Queue should not allow to push element when exceeds limit");
            q.Circular = true;
            Assert.IsTrue(q.Push("GHI"), "Queue should allow to push element when circular mode is on");
            Assert.AreEqual("DEF", q.Pull());
            Assert.AreEqual("GHI", q.Pull());

            DateTime start;
            TimeSpan spend;

            start = DateTime.Now;
            Assert.AreEqual(null, q.Pull());
            spend = DateTime.Now - start;
            Assert.IsTrue(spend < TimeSpan.FromSeconds(0.1), "Time spend should be less than 0.1 s");

            start = DateTime.Now;
            Assert.AreEqual(null, q.Pull(0.2));
            spend = DateTime.Now - start;
            Assert.IsTrue(spend >= TimeSpan.FromSeconds(0.1), "Time spend should be at least more than 0.1 s");

            start = DateTime.Now;
            Thread t1 = new Thread(() => { Thread.Sleep(10); q.Push("123"); });
            t1.Start();
            Assert.AreEqual("123", q.Pull(0.3));
            spend = DateTime.Now - start;
            Assert.IsTrue(spend >= TimeSpan.FromMilliseconds(10), "Time spend should be at least 10 ms");

            q.Push("XYZ");
            Assert.AreEqual(1, q.Count);
            q.Clear();
            Assert.AreEqual(0, q.Count);
        }
   }
}