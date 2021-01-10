using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using Energy.Interface;

namespace Energy.Core
{
    /// <summary>
    /// Thread worker generic base class.
    /// <br /><br />
    /// Override Work method in derrived class.
    /// </summary>
    public class Worker<T> : Energy.Interface.IWorker
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

        public Worker(T state)
        {
            State = state;
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
        private bool GetRunning()
        {
            lock (_ThreadLock)
            {
                if (_Thread == null)
                {
                    return false;
                }
#if !NETCF
                if (_Thread.IsAlive)
                {
                    return true;
                }
#endif
#if NETCF
                // Compact Framework doesn't have IsAlive :-(
                if (_Thread != null)
                {
                    return true;
                }
#endif
                return false;
            }
        }

        /// <summary>
        /// Is thread running?
        /// </summary>
        public bool Running { get { return GetRunning(); } }

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

        public ManualResetEvent StoppedResetEvent = new ManualResetEvent(false);

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
                if (value)
                {
                    StoppedResetEvent.Set();
                }
                else
                {
                    StoppedResetEvent.Reset();
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
#if !NETCF
                        return _Thread.CurrentCulture;
#endif
#if NETCF
                        // Compact Framework doesn't support this property in Thread class
                        return _CurrentCulture;
#endif
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
#if !NETCF
                        _Thread.CurrentCulture = value;
                        _Thread.CurrentUICulture = value;
#endif
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

        public virtual void Start()
        {
            lock (_ThreadLock)
            {
                if (null != _Thread)
                {
#if !NETCF
                    if (_Thread.IsAlive)
                    {
                        return;
                    }
#endif
                }
                _Thread = new System.Threading.Thread(Work)
                {
                    IsBackground = _Background
                };
                if (null != _CurrentCulture)
                {
#if !NETCF
                    _Thread.CurrentCulture = _CurrentCulture;
                    _Thread.CurrentUICulture = _CurrentCulture;
#endif
                }
                _Stopped = false;
                _LastStart = DateTime.Now;
                try
                {
                    _Thread.Start();
                }
                catch (Exception x)
                {
                    Core.Bug.Write("EC505", x);
                    throw;
                }
            }
        }

        public virtual void Stop()
        {
            Stopped = true;
        }

        /// <summary>
        /// Wait for thread to finish work for specified time.
        /// Returns true if thread exited and false if still running.
        /// </summary>
        /// <param name="time">Time in milliseconds</param>
        /// <returns>True if thread exited, false if still running</returns>
        public bool Wait(int time)
        {
            return Energy.Core.Worker.Wait(_Thread, time);
        }

        /// <summary>
        /// Wait for thread to finish work for specified time.
        /// Returns true if thread exited and false if still running.
        /// </summary>
        /// <param name="time">Time in seconds</param>
        /// <returns>True if thread exited, false if still running</returns>
        public bool Wait(double time)
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
                {
                    return;
                }
#if !NETCF
                if (!_Thread.IsAlive)
                {
                    _Thread = null;
                    return;
                }
#endif
                try
                {
                    _Thread.Abort();
                    //_Thread.Interrupt();
                }
                catch (PlatformNotSupportedException)
                {

                }
            }
        }

        /// <summary>
        /// Stop thread until the end. 
        /// Execution will be resumed after Stopped will be set to true which usually comes after Stop.
        /// </summary>
        public void Sleep()
        {
            StoppedResetEvent.WaitOne();
        }

        /// <summary>
        /// Sleep for specific time or until stop whatever comes first.
        /// Returns false if stopped signal was received.
        /// </summary>
        /// <param name="time">Time in milliseconds to sleep</param>
        /// <returns>
        /// Returns false if stopped signal was received. 
        /// Returns true when specific amout of time just passed.
        /// </returns>
        public bool Sleep(int time)
        {
            return !StoppedResetEvent.WaitOne(time, true);
        }

        /// <summary>
        /// Sleep for specific time or until stop whatever comes first.
        /// Returns false if stopped signal was received.
        /// </summary>
        /// <param name="time">Time in milliseconds to sleep</param>
        /// <returns>
        /// Returns false if stopped signal was received. 
        /// Returns true when specific amout of time just passed.
        /// </returns>
        public bool Sleep(double time)
        {
            return Sleep((int)(1000 * time));
        }

        #endregion
    }

    /// <summary>
    /// Worker thread utility class
    /// </summary>
    public static class Worker
    {
        #region Class

        #region Pool

        public class Pool<T> : Energy.Interface.IPool
        {
            public T Context { get; set; }

            private List<T> _List = new List<T>();

            public T[] Array { get { return GetArray(); } }

            public bool Running { get { return GetRunning(); } }

            public int Count { get { return GetCount(); } }

            private bool GetRunning()
            {
                foreach (T o in GetArray())
                {
                    if (null == o)
                    {
                        continue;
                    }
                    if (o is Energy.Interface.IWorker)
                    {
                        Energy.Interface.IWorker w = o as Energy.Interface.IWorker;
                        if (w.Running)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }

            private int GetCount()
            {
                return _List.Count;
            }

            public void Add(T o)
            {
                _List.Add(o);
            }

            public void Remove(T o)
            {
                _List.Remove(o);
            }

            public List<T> FindAll(Predicate<T> match)
            {
                return _List.FindAll(match);
            }

            public T Find(Predicate<T> match)
            {
                return _List.Find(match);
            }

            private T[] GetArray()
            {
                return _List.ToArray();
            }

            public void Spawn()
            {
                try
                {
                    T o = Activator.CreateInstance<T>();
                    _List.Add(o);
                    if (o is Energy.Interface.IWorker)
                    {
                        Energy.Interface.IWorker worker = o as Energy.Interface.IWorker;
                        worker.Start();
                    }
                }
                catch (MissingMethodException)
                {
                    System.Diagnostics.Debug.WriteLine("Missing method for constructing new object for spawn");
                }
            }

            public void Start()
            {
                foreach (T o in GetArray())
                {
                    if (null == o)
                    {
                        continue;
                    }
                    if (o is Energy.Interface.IWorker)
                    {
                        Energy.Interface.IWorker w = o as Energy.Interface.IWorker;
                        w.Start();
                    }
                }
            }

            public void Stop()
            {
                foreach (T o in GetArray())
                {
                    if (null == o)
                    {
                        continue;
                    }
                    if (o is Energy.Interface.IWorker)
                    {
                        Energy.Interface.IWorker w = o as Energy.Interface.IWorker;
                        w.Stop();
                    }
                }
            }

            /// <summary>
            /// Purge worker pool by removing all workers that are currently stopped (not working).
            /// <br /><br />
            /// Returns amount of workers removed this way.
            /// </summary>
            /// <returns></returns>
            public int Purge()
            {
                int count = 0;
                foreach (T o in GetArray())
                {
                    if (null == o)
                    {
                        continue;
                    }
                    bool remove = false;
                    if (o is Energy.Interface.IWorker)
                    {
                        Energy.Interface.IWorker w = o as Energy.Interface.IWorker;
                        if (!w.Running)
                        {
                            remove = true;
                        }
                    }
                    if (remove)
                    {
                        Remove(o);
                        count++;
                    }
                }
                return count;
            }
        }

        public class Pool : Pool<object> { }

        #endregion

        #region Simple

        public abstract class Simple<T> : Worker<T>
        {
            public abstract override void Work();
        }

        public abstract class Simple : Simple<object> { }

        #endregion

        #region Loop

        public abstract class Loop<T> : Worker<T>
        {
            public abstract override void Work();
        }

        public abstract class Loop : Loop<object> { }

        #endregion

        #region Event

        public abstract class Event<T> : Worker<T>
        {
            public abstract override void Work();
        }

        public abstract class Event : Event<object> { }

        #endregion

        #endregion

        #region Utility

        #region Wait

        /// <summary>
        /// Wait for thread to finish work for specified time in seconds.
        /// </summary>
        /// <param name="thread">Thread object</param>
        /// <param name="time">Time in seconds</param>
        /// <returns>True if thread exited, false if still running</returns>
        public static bool Wait(System.Threading.Thread thread, double time)
        {
            return Wait(thread, (int)1000 * time);
        }

/*
 * Code commented out, because it's not proper way to wait for a thread anymore with separate guardian thread.
 * Aborting thread is no longer supported (.NET Core).
 *

        /// <summary>
        /// Wait for thread to finish work for specified time in milliseconds.
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
                IsBackground = true,
            };
            guardian.Start();
            bool success = manualResetEvent.WaitOne(time);
            if (success)
            {
                return true;
            }
            else
            {
                try
                {
                    guardian.Abort();
                }
                catch (PlatformNotSupportedException xPlatformNotSupportedException)
                {
                    System.Diagnostics.Debug.WriteLine(Energy.Core.Bug.GetExceptionMessage(xPlatformNotSupportedException, true));
                }
                return false;
            }
        }
 */

        /// <summary>
        /// Wait for thread to finish work for specified time in milliseconds.
        /// </summary>
        /// <param name="thread">Thread object</param>
        /// <param name="time">Time in milliseconds</param>
        /// <returns>True if thread exited, false if still running</returns>
        public static bool Wait(System.Threading.Thread thread, int time)
        {
            if (null == thread)
            {
                return true;
            }
#if !NETCF
            else if (!thread.IsAlive)
            {
                return true;
            }
#endif
            else
            {
                bool success = thread.Join(time);
                return success;
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
        public static Thread Fire(Energy.Base.Anonymous.Action code)
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
            {
                IsBackground = true,
            };
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
        public static Thread Fire(string name, Energy.Base.Anonymous.Action code)
        {
            lock (_FireThreadListLock)
            {
                Thread thread;
                if (FireThreadList.ContainsKey(name))
                {
                    thread = FireThreadList[name];
#if !NETCF
                    if (thread.IsAlive)
                    {
                        return thread;
                    }
#endif
#if NETCF
                    if (thread != null)
                    {
                        return thread;
                    }
#endif
                    FireThreadList.Remove(name);
                }
                thread = Fire(code);
                FireThreadList[name] = thread;
                return thread;
            }
        }

        #endregion

        #region RemoveUnused

        public static object[] RemoveUnused(object[] workerArray
            , Energy.Base.Anonymous.Action<object> onRemove
            , Energy.Base.Anonymous.Action<Exception> onException
            )
        {
            if (null == workerArray || 0 == workerArray.Length)
            {
                return workerArray;
            }
            List<object> result = new List<object>();
            for (int i = 0; i < workerArray.Length; i++)
            {
                Energy.Interface.IWorker worker = workerArray[i] as Energy.Interface.IWorker;
                if (null != worker)
                {
                    if (!worker.Stopped || worker.Running)
                    {
                        result.Add(worker);
                        continue;
                    }
                    try
                    {
                        if (null != onRemove)
                        {
                            onRemove(worker);
                        }
                    }
                    catch (Exception x)
                    {
                        if (null != onException)
                        {
                            onException(x);
                        }
                    }
                }
            }
            return result.ToArray();
        }

        #endregion

        #region StopRunning

        public static void StopRunning(object[] workerArray
            , Energy.Base.Anonymous.Action<object> onStopped
            , Energy.Base.Anonymous.Action<Exception> onException
            )
        {
            if (null == workerArray)
            {
                return;
            }
            for (int i = 0; i < workerArray.Length; i++)
            {
                Energy.Interface.IWorker worker = workerArray[i] as Energy.Interface.IWorker;
                if (null != worker)
                {
                    if (!worker.Running)
                    {
                        continue;
                    }
                    try
                    {
                        worker.Stop();
                        if (null != onStopped)
                        {
                            onStopped(worker);
                        }
                    }
                    catch (Exception x)
                    {
                        if (null != onException)
                        {
                            onException(x);
                        }
                    }
                }
            }
        }

        #endregion

        #endregion
    }
}
