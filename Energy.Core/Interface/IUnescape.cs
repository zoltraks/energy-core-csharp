namespace Energy.Interface
{
    /// <summary>
    /// Interface for escaping texts with quotes or surrounded in any way.
    /// </summary>
    public interface IUnescape
    {
        string Unescape(string text);
    }
}