namespace Energy.Interface
{
    /// <summary>
    /// Interface for escaping texts with quotes or surrounded in any way.
    /// </summary>
    public interface IQueue<T>
    {
        /// <summary>
        /// Put element at the end of queue.
        /// </summary>
        /// <param name="item">Element</param>
        void Push(T item);

        /// <summary>
        /// Put array of elements at the end of queue.
        /// </summary>
        /// <param name="array"></param>
        void Push(T[] array);

        /// <summary>
        /// Take element from queue, remove it from queue, and finally return.
        /// </summary>
        /// <returns>Element</returns>
        T Pull();

        /// <summary>
        /// Take number of elements from queue, remove them and return array of elements taken.
        /// Take(0) will return all elements from queue and empty it.
        /// </summary>
        /// <param name="count">Number of elements</param>
        /// <returns>Array of elements</returns>
        T[] Pull(int count);

        /// <summary>
        /// Put element back to queue. This element will be taken first.
        /// </summary>
        /// <param name="item">Element</param>
        void Back(T item);

        /// <summary>
        /// Put array of elements back to queue. These elements will be taken first.
        /// </summary>
        /// <param name="list">Array of elements</param>
        void Back(T[] list);

        /// <summary>
        /// Delete last element from queue and return it.
        /// </summary>
        /// <returns>Element or default if queue was empty</returns>
        T Chop();

        /// <summary>
        /// Delete number of last elements from queue and return them.
        /// </summary>
        /// <param name="count">Number of elements</param>
        /// <returns>Array of elements</returns>
        T[] Chop(int count);
    }
}