using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;

namespace Energy.Core
{
    /// <summary>
    /// Thread worker generic base class.
    /// Override Work method in derrived class.
    /// </summary>
    public class Worker<T> : Energy.Interface.IWork
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Worker()
        {
        }

        public static Worker<T> Create()
        {
            return new Worker<T>();
        }

        #endregion

        #region Property

        private readonly object _ThreadLock = new object();

        private DateTime _LastStart;
        /// <summary>
        /// Time of last start of execution
        /// </summary>
        public DateTime LastStart
        {
            get
            {
                return _LastStart;
            }
            private set
            {
                _LastStart = value;
            }
        }

        /// <summary>
        /// Is thread running?
        /// </summary>
        public bool IsRunning
        {
            get
            {
                lock (_ThreadLock)
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
        /// <summary>
        /// Represents state object.
        /// Thread safety must be implemented by object itself if needed.
        /// </summary>
        public T State
        {
            get
            {
                return _State;
            }
            set
            {
                _State = value;
            }
        }

        private bool _Stopped = true;
        /// <summary>
        /// Stopped state
        /// </summary>
        public bool Stopped
        {
            get
            {
                lock (_ThreadLock)
                {
                    return _Stopped;
                }
            }
            set
            {
                lock (_ThreadLock)
                {
                    _Stopped = value;
                }
            }
        }

        private bool _Background = true;
        /// <summary>
        /// Run thread in background. 
        /// Otherwise block program execution until thread is stopped.
        /// </summary>
        public bool Background
        {
            get
            {
                lock (_ThreadLock)
                {
                    return _Background;
                }
            }
            set
            {
                lock (_ThreadLock)
                {
                    _Background = value;
                    if (null != _Thread)
                    {
                        _Thread.IsBackground = value;
                    }
                }
            }
        }

        private CultureInfo _CurrentCulture;
        /// <summary>
        /// Run thread in background. 
        /// Otherwise block program execution until thread is stopped.
        /// </summary>
        public CultureInfo CurrentCulture
        {
            get
            {
                lock (_ThreadLock)
                {
                    if (null != _Thread)
                    {
                        return _Thread.CurrentCulture;
                    }
                    else
                    {
                        return _CurrentCulture;
                    }
                }
            }
            set
            {
                lock (_ThreadLock)
                {
                    _CurrentCulture = value;
                    if (null != _Thread)
                    {
                        _Thread.CurrentCulture = value;
                        _Thread.CurrentUICulture = value;
                    }
                }
            }
        }

        private System.Threading.Thread _Thread;
        /*
        /// <summary>
        /// Represents working thread
        /// </summary>
        public System.Threading.Thread Thread
        {
            get
            {
                return _Thread;
            }
            private set
            {
                _Thread = value;
            }
        }
        */

        #endregion

        #region Function

        /// <summary>
        /// Override this method to add implementation for worker process.
        /// Do not call base.Work() in overriden method.
        /// </summary>
        public virtual void Work()
        {
            throw new NotImplementedException();
        }

        public virtual bool Start()
        {
            lock (_ThreadLock)
            {
                if (null != _Thread)
                {
                    if (_Thread.IsAlive)
                    {
                        return false;
                    }
                }
                _Thread = new System.Threading.Thread(Work)
                {
                    IsBackground = _Background
                };
                if (null != _CurrentCulture)
                {
                    _Thread.CurrentCulture = _CurrentCulture;
                    _Thread.CurrentUICulture = _CurrentCulture;
                }
                _Stopped = false;
                _LastStart = DateTime.Now;
                try
                {
                    _Thread.Start();
                    return true;
                }
                catch (Exception x)
                {
                    Core.Bug.Write("EC505", x);
                    return false;
                }
            }
        }

        public virtual void Stop()
        {
            Stopped = true;
        }

        /// <summary>
        /// Wait for thread to finish work for specified time.
        /// </summary>
        /// <param name="time">Time in milliseconds</param>
        /// <returns>True if thread exited, false if still running</returns>
        public bool Wait(int time)
        {
            return Energy.Core.Worker.Wait(_Thread, time);
        }

        /// <summary>
        /// Abort thread process.
        /// Raises a System.Threading.ThreadAbortException to begin the process of terminating the thread.
        /// Calling this method usually terminates the thread.
        /// </summary>
        public void Abort()
        {
            lock (_ThreadLock)
            {
                _Stopped = true;
                if (null == _Thread)
                    return;
                if (!_Thread.IsAlive)
                {
                    _Thread = null;
                    return;
                }
                _Thread.Abort();
            }
        }

        #endregion
    }

    /// <summary>
    /// Worker thread utility class
    /// </summary>
    public static class Worker
    {
        #region Class

        #region Simple

        public abstract class Simple<T> : Worker<T>
        {
            public abstract override void Work();
        }

        public abstract class Simple : Simple<object>
        {
        }

        #endregion

        #region Loop

        public class Loop<T> : Worker<T>
            {

            }
            public class Loop : Loop<object>
            {

            }

            #endregion

        #region Event

        public class Event<T> : Worker<T>
        {

        }

        public class Event : Event<object>
        {

        }

        #endregion

        #endregion

        #region Utility

        #region Wait

        /// <summary>
        /// Wait for thread to finish work for specified time.
        /// </summary>
        /// <param name="thread">Thread object</param>
        /// <param name="time">Time in milliseconds</param>
        /// <returns>True if thread exited, false if still running</returns>
        public static bool Wait(System.Threading.Thread thread, int time)
        {
            if (null == thread || !thread.IsAlive)
            {
                return true;
            }
            System.Threading.ManualResetEvent manualResetEvent = new System.Threading.ManualResetEvent(false);
            System.Threading.Thread guardian = new System.Threading.Thread(() =>
            {
                try
                {
                    thread.Join();
                    manualResetEvent.Set();
                }
                catch (System.Threading.ThreadAbortException)
                {
                }
            })
            {
                IsBackground = true
            };
            guardian.Start();
            bool success = manualResetEvent.WaitOne(time);
            if (success)
            {
                return true;
            }
            else
            {
                guardian.Abort();
                return false;
            }
        }

        #endregion

        #region Fire

        private static readonly Energy.Base.Lock _FireThreadListLock = new Energy.Base.Lock();

        private static Dictionary<string, Thread> _FireThreadList;

        private static Dictionary<string, Thread> FireThreadList
        {
            get
            {
                if (_FireThreadList == null)
                {
                    _FireThreadList = new Dictionary<string, Thread>();
                }
                return _FireThreadList;
            }
        }

        /// <summary>
        /// Start new thread with anonymous code.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Thread Fire(Energy.Base.Anonymous.Function code)
        {
            Thread thread = new Thread(() =>
            {
                try
                {
                    code();
                }
                catch (ThreadAbortException)
                { }
            })
            { IsBackground = true };
            thread.Start();
            return thread;
        }

        /// <summary>
        /// Create and start named thread with anonymous code.
        /// Don't do anything if named thread exists and is running.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Thread Fire(string name, Energy.Base.Anonymous.Function code)
        {
            lock (_FireThreadListLock)
            {
                Thread thread;
                if (FireThreadList.ContainsKey(name))
                {
                    thread = FireThreadList[name];
                    if (thread.IsAlive)
                    {
                        return thread;
                    }
                    else
                    {
                        FireThreadList.Remove(name);
                    }
                }
                thread = Fire(code);
                FireThreadList[name] = thread;
                return thread;
            }
        }

        #endregion

        #endregion
    }
}
