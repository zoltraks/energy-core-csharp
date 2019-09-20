using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace WorkerSimpleControl.Worker
{
    public class Service1 : Energy.Core.Worker.Simple
    {
        public override void Work()
        {
            while (!Stopped)
            {
                if (this.StoppedResetEvent.WaitOne(1000))
                {
                    Debug.WriteLine(Energy.Base.Clock.CurrentTimeSpace + "Wait returned True");
                    break;
                }
            }
        }

        public override void Stop()
        {
            Debug.WriteLine(Energy.Base.Clock.CurrentTimeSpace + "Stop invoked");
            base.Stop();
        }
    }
}
