namespace Energy.Interface
{
    public interface IRow<TKey, TValue>
    {
        /// <summary>
        /// Value access by index number
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        TValue this[int index] { get; set; }

        /// <summary>
        /// Value access by column key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        TValue this[TKey key] { get; set; }

        IRow<TKey, TValue> Append(TKey key, TValue value);

        TValue Get(int index);

        TValue Get(TKey key);

        IRow<TKey, TValue> Set(int index, TValue value);

        IRow<TKey, TValue> Set(TKey key, TValue value);
    }
}