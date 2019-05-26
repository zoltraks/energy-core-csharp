using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Core
{
    [TestClass]
    public class Worker
    {
        [TestMethod]
        public void WorkerStartStop()
        {
            Class.Worker1 worker1 = new Class.Worker1();
            Class.Worker2 worker2 = new Class.Worker2();
            Assert.IsTrue(worker1.Start());
            Assert.IsTrue(worker2.Start());
            worker1.Stop();
            worker2.Stop();
            Assert.IsTrue(worker1.Wait(10));
            Assert.IsTrue(worker2.Wait(10));
        }

        public class Class
        {
            public class Worker1: Energy.Core.Worker<object>
            {
                public override void Work()
                {
                    this.State = "OK";
                }
            }

            public class Worker2: Energy.Core.Worker.Simple
            {
                public override void Work()
                {
                    this.State = "OK";
                }
            }
        }
    }
}
