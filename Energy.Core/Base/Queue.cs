using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    public class Queue<T>: IDisposable, Energy.Interface.IQueue<T>
    {
        private System.Collections.Generic.List<T> _List = new System.Collections.Generic.List<T>();

        public Queue()
        {
        }

        public Queue(T[] list)
        {
            _List.AddRange(list);
        }

        public void Dispose()
        {
            lock (_List)
            {
                _List.Clear();
                _List = null;
            }
        }

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

        /// <summary>
        /// Put element at the end of queue.
        /// </summary>
        /// <param name="item">Element</param>
        public void Push(T item)
        {
            lock (_List)
            {
                _List.Add(item);
            }
        }

        /// <summary>
        /// Put array of elements at the end of queue.
        /// </summary>
        /// <remarks>Using one element instead of array may be more efficient.</remarks>
        /// <param name="array">Array of elements</param>
        public void Push(T[] array)
        {
            lock (_List)
            {
                _List.AddRange(array);
            }
        }

        /// <summary>
        /// Take first element from queue, remove it from queue, and finally return.
        /// </summary>
        /// <returns>Element</returns>
        public T Take()
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

        /// <summary>
        /// Take a number of elements from queue, remove them and return array of elements taken.
        /// Take(0) will return all elements from queue and empty it.
        /// </summary>
        /// <param name="count">Number of elements</param>
        /// <returns>Array of elements</returns>
        public T[] Take(int count)
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

        /// <summary>
        /// Put element back to queue, at begining. This element will be taken first.
        /// </summary>
        /// <param name="item">Element</param>
        public void Back(T item)
        {
            lock (_List)
            {
                _List.Insert(0, item);
            }
        }

        /// <summary>
        /// Put array of elements back to queue, at begining. These elements will be taken first.
        /// </summary>
        /// <param name="list">Array of elements</param>
        public void Back(T[] list)
        {
            lock (_List)
            {
                for (int i = 0; i < list.Length; i++)
                {
                    _List.Insert(i, list[i]);
                }
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
    }
}
