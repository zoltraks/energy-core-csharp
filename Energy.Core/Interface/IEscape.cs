namespace Energy.Interface
{
    /// <summary>
    /// Interface for escaping texts with quotes or surrounded in any way.
    /// </summary>
    public interface IEscape
    {
        string Escape(string text);
    }
}