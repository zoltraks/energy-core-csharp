using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Core
{
    /// <summary>
    /// Thread worker base class for making threading even simpler
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

        /// <summary>Is thread still running?</summary>
        public bool IsRunning
        {
            get
            {
                if (_Thread == null)
                    return false;
                if (_Thread.IsAlive)
                    return true;
                return false;
            }
        }

        private T _State;
        private readonly object _StateLock = new object();
        /// <summary>State</summary>
        public T State {
            get { lock (_StateLock) return _State; }
            set { lock (_StateLock) _State = value; }
        }

        private System.Threading.Thread _Thread;

        /// <summary>Thread</summary>
        public System.Threading.Thread Thread {
            get { return _Thread; }
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

        }
    }

    public class Worker : Worker<object> { }
}
