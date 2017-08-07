using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Core
{
    /// <summary>
    /// Thread worker base class for making threading even simpler.
    /// </summary>
    public class Worker<T>
    {
        private DateTime _LastStartTime;
        private readonly object _StartTimeLock = new object();

        /// <summary>Time of last start of execution</summary>
        public DateTime LastStartTime {
            get { lock (_StartTimeLock) return _LastStartTime; }
            private set { lock (_StartTimeLock) _LastStartTime = value; }
        }

        private readonly object _IsRunningLock = new object();
        /// <summary>Is thread still running?</summary>
        public bool IsRunning
        {
            get
            {
                lock (_IsRunningLock)
                {
                    if (_Thread == null)
                        return false;
                    if (_Thread.IsAlive)
                        return true;
                    return false;
                }
            }
        }

        private T _State;
        private readonly object _StateLock = new object();
        /// <summary>State</summary>
        public T State {
            get { lock (_StateLock) return _State; }
            set { lock (_StateLock) _State = value; }
        }

        private bool _Stopped = true;
        private readonly object _StoppedLock = new object();
        /// <summary>Stopped</summary>
        public bool Stopped { get { lock (_StoppedLock) return _Stopped; } set { lock (_StoppedLock) _Stopped = value; } }

        private System.Threading.Thread _Thread;
        /// <summary>Thread</summary>
        public System.Threading.Thread Thread {
            get { return _Thread; }
            private set { _Thread = value; }
        }

        public Worker()
        {
        }

        public static Worker<T> Create()
        {
            Worker<T> _ = new Worker<T>();
            return _;
        }

        public void Start()
        {
            if (IsRunning)
                return;
            this.Stopped = false;
            System.Threading.Thread thread = new System.Threading.Thread(Work);
            thread.Start(this);
            this.Thread = thread;
        }

        public void Stop()
        {
            Stopped = true;
        }

        public virtual void Work(object parameter)
        {
            throw new NotImplementedException();
        }

        public bool WaitForExit(int time)
        {
            return Worker.WaitForExit(_Thread, time);
        }
    }

    public class Worker : Worker<object>
    {
        public class Loop<T>: Worker<T>
        {

        }

        /// <summary>
        /// Wait for thread exit, return true if exited, false if still running.
        /// </summary>
        /// <param name="thread"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static bool WaitForExit(System.Threading.Thread thread, int time)
        {
            if (thread == null || !thread.IsAlive)
                return true;
            System.Threading.ManualResetEvent manualResetEvent = new System.Threading.ManualResetEvent(false);
            WaitForExitParameter parameter = new Core.Worker.WaitForExitParameter()
            {
                ManualResetEvent = manualResetEvent,
                Thread = thread,
            };
            System.Threading.Thread guardian = new System.Threading.Thread(WaitForParameter);
            guardian.Start(parameter);
            bool success = manualResetEvent.WaitOne(time);
            if (!success)
            {
                guardian.Abort();
                return false;
            }
            return true;
        }

        private static void WaitForParameter(object parameter)
        {
            WaitForExitParameter data = parameter as WaitForExitParameter;
            try
            {
                data.Thread.Join();
                data.Result = true;
                data.ManualResetEvent.Set();
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
        }

        private class WaitForExitParameter
        {
            public System.Threading.ManualResetEvent ManualResetEvent;

            public System.Threading.Thread Thread;

            public bool Result;
        }
    }
}
