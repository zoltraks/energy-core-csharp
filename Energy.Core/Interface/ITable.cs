namespace Energy.Interface
{
    public interface ITable<TKey, TValue>
    {
        /// <summary>
        /// Row access by index number
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        IRow<TKey, TValue> this[int index] { get; set; }

        /**
         * Disabled for now
         **/
        ///// <summary>
        ///// Row access by record key
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //IRow<TKey, TValue> this[TKey key] { get; set; }

        Energy.Interface.IRow<TKey, TValue> New();
    }
}