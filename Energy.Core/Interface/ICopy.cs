namespace Energy.Interface
{
    public interface ICopy<T>
    {
        /// <summary>
        /// Make copy of object
        /// </summary>
        /// <returns></returns>
        T Copy();
    }

    public interface ICopy: ICopy<object>
    {
    }
}
