using System;
using System.Collections.Generic;
using System.Threading;

namespace Energy.Base
{
    /// <summary>
    /// Generic thread-safe class which can be used to implement FIFO (first-in first-out) queues.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Queue<T>: IDisposable, Energy.Interface.IQueue<T>
    {
        #region Constructor

        public Queue()
        {
            _PushResetEvent = new ManualResetEvent(false);
        }

        public Queue(T[] list)
            : this()
        {
            _List.AddRange(list);
        }

        #endregion

        #region Property

        /// <summary>
        /// Check if queue is empty.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                lock (_List)
                {
                    return _List.Count == 0;
                }
            }
        }

        private int _Limit;

        /// <summary>
        /// Limit number of items in queue.
        /// </summary>
        public int Limit { get { lock (_List) return _Limit; } set { lock (_List) _Limit = value; } }

        private bool _Circular;

        /// <summary>
        /// Circular mode. When this option is set, 
        /// the oldest items are removed from the list when limit has been exceeded.
        /// </summary>
        public bool Circular { get { lock (_List) return _Circular; } set { lock (_List) _Circular = value; } }

        /// <summary>
        /// Number of elements in queue.
        /// </summary>
        public int Count
        {
            get
            {
                lock (_List)
                {
                    return _List.Count;
                }
            }
        }

        #endregion

        #region Private

        private System.Collections.Generic.List<T> _List = new System.Collections.Generic.List<T>();

        private ManualResetEvent _PushResetEvent;

        #endregion

        #region Dispose

        public void Dispose()
        {
            lock (_List)
            {
                _List.Clear();
                _List = null;
            }
        }

        #endregion

        #region Clear

        /// <summary>
        /// Clear list.
        /// </summary>
        public void Clear()
        {
            lock (_List)
            {
                _List.Clear();
            }
        }

        #endregion

        #region Push

        /// <summary>
        /// Put element at the end of queue.
        /// If limit is reached, function will return false and element will not be put
        /// at the end of the queue unless Circular option is set.
        /// </summary>
        /// <param name="item">Element</param>
        /// <returns></returns>
        public bool Push(T item)
        {
            try
            {
                lock (_List)
                {
                    if (Limit > 0 && _List.Count >= Limit)
                    {
                        if (Circular)
                        {
                            int count = 1 + Limit - _List.Count;
                            _List.RemoveRange(0, count);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    _List.Add(item);
                    return true;
                }
            }
            finally
            {
                _PushResetEvent.Set();
            }
        }

        /// <summary>
        /// Put array of elements at the end of queue.
        /// </summary>
        /// <remarks>
        /// Using one element instead of array may be more efficient.
        /// </remarks>
        /// <param name="array">Array of elements</param>
        /// <returns></returns>
        public bool Push(T[] array)
        {
            if (array == null || array.Length == 0)
                return false;
            try
            {
                lock (_List)
                {
                    if (Limit > 0 && _List.Count + array.Length > Limit)
                    {
                        if (Circular)
                        {
                            int count = 1 + Limit - _List.Count - array.Length;
                            _List.RemoveRange(0, count);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    _List.AddRange(array);
                    return false;
                }
            }
            finally
            {
                _PushResetEvent.Set();
            }
        }

        #endregion

        #region Pull

        /// <summary>
        /// Take first element from queue, remove it from queue, and finally return.
        /// If queue is empty, function will return null.
        /// </summary>
        /// <returns>Element</returns>
        public T Pull()
        {
            try
            {
                lock (_List)
                {
                    if (_List.Count == 0)
                        return default(T);
                    T item = _List[0];
                    _List.RemoveAt(0);
                    return item;
                }
            }
            finally
            {
                //_PushResetEvent.Reset();
            }
        }

        /// <summary>
        /// Take number of elements from queue, remove them and return array of elements taken.
        /// Pull(0) will return all elements from queue and empty it.
        /// </summary>
        /// <param name="count">Number of elements</param>
        /// <returns>Array of elements</returns>
        public T[] Pull(int count)
        {
            try
            {
                lock (_List)
                {
                    int max = _List.Count;
                    if (count == 0 || count > max)
                        count = max;
                    System.Collections.Generic.List<T> list = new System.Collections.Generic.List<T>();
                    while (count-- > 0)
                    {
                        list.Add(_List[0]);
                        _List.RemoveAt(0);
                    }
                    return list.ToArray();
                }
            }
            finally
            {
                //_PushResetEvent.Reset();
            }
        }

        /// <summary>
        /// Take element from queue with specified time limit to wait
        /// for new item to come.
        /// It will pause invoking thread as it is expected to do so.
        /// </summary>
        /// <param name="timeout">Time limit in seconds</param>
        /// <returns>Element or default (null) if no elements in queue</returns>
        public T Pull(double timeout)
        {
            try
            {
                lock (_List)
                {
                    if (_List.Count != 0)
                    {
                        T item = _List[0];
                        _List.RemoveAt(0);
                        return item;
                    }
                }

                _PushResetEvent.Reset();

                if (!_PushResetEvent.WaitOne(TimeSpan.FromSeconds(timeout)))
                {
                    return default(T);
                }
                else
                {
                    lock (_List)
                    {
                        if (_List.Count == 0)
                        {
                            return default(T);
                        }
                        else
                        {
                            T item = _List[0];
                            _List.RemoveAt(0);
                            return item;
                        }
                    }
                }
            }
            finally
            {
                //_PushResetEvent.Reset();
            }
        }

        #endregion

        #region Back

        /// <summary>
        /// Put element back to queue, at begining. 
        /// This element will be taken first.
        /// </summary>
        /// <param name="item">Element</param>
        public void Back(T item)
        {
            try
            {
                lock (_List)
                {
                    _List.Insert(0, item);

                }
            }
            finally
            {
                //_PushResetEvent.Set();
            }
        }

        /// <summary>
        /// Put array of elements back to queue, at begining.
        /// These elements will be taken first.
        /// </summary>
        /// <param name="list">Array of elements</param>
        public void Back(T[] list)
        {
            try
            {
                lock (_List)
                {
                    for (int i = 0; i < list.Length; i++)
                    {
                        _List.Insert(i, list[i]);
                    }
                }
            }
            finally
            {
               // _PushResetEvent.Set();
            }
        }

        #endregion

        #region Chop

        /// <summary>
        /// Delete last element from queue and return it.
        /// </summary>
        /// <returns>Element or default if queue was empty</returns>
        public T Chop()
        {
            lock (_List)
            {
                if (_List.Count == 0)
                    return default(T);
                int n = _List.Count - 1;
                T last = _List[n];
                _List.RemoveAt(n);
                return last;
            }
        }

        /// <summary>
        /// Delete number of last elements from queue and return them.
        /// </summary>
        /// <param name="count">Number of elements</param>
        /// <returns>Array of elements</returns>
        public T[] Chop(int count)
        {
            lock (_List)
            {
                if (count > _List.Count)
                    count = _List.Count;
                if (count == 0)
                    return new T[] { };
                int first = _List.Count - count;
                List<T> list = _List.GetRange(first, count);
                _List.RemoveRange(first, count);
                return list.ToArray();
            }
        }

        #endregion
    }

    /// <summary>
    /// Non generic thread-safe class which can be used to implement FIFO (first-in first-out) queues.
    /// </summary>
    public class Queue: Queue<object>
    {
    }
}
