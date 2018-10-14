using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AsynchronousNetworkClient
{
    public class SimpleClient
    {
        public Thread Thread;

        public volatile bool Stopped;

        public string Address = "localhost";

        public int Port = 9000;

        public void Start()
        {
            if (Thread != null && Thread.IsAlive)
            {
                Thread.Abort();
            }
            Stopped = false;
            Thread = new Thread(Work);
            Thread.Start();
        }

        // ManualResetEvent instances signal completion.
        private static ManualResetEvent connectDone =
            new ManualResetEvent(false);
        private static ManualResetEvent sendDone =
            new ManualResetEvent(false);
        private static ManualResetEvent receiveDone =
            new ManualResetEvent(false);

        private void Work()
        {
            try
            {
                while (!Stopped)
                {
                    Energy.Core.Tilde.WriteLine("Connecting to ~y~" + Address + "~0~ port ~c~" + Port);
                }
            }
            catch (ThreadAbortException)
            {
                Cancel();
            }
            finally
            {
                Stopped = true;
            }
        }

        private void Cancel()
        {
            throw new NotImplementedException();
        }
    }
}
