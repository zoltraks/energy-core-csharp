namespace Energy.Interface
{
    /// <summary>
    /// Interface for string lists and arrays.
    /// </summary>
    public interface IStringList
    {
        int Count { get; }
        int TotalLength { get; }

        bool HasDuplicates();
        bool HasDuplicates(bool ignoreCase);

        int IndexOf(string element);
        int IndexOf(string element, int index);
        int IndexOf(string element, bool ignoreCase);
        int IndexOf(string element, bool ignoreCase, int index);
    }
}