namespace Energy.Interface
{
    /// <summary>
    /// Interface for string lists and arrays.
    /// </summary>
    public interface IStringList
    {
        int TotalLength { get; }

        bool HasDuplicates();
        bool HasDuplicates(bool ignoreCase);
    }
}