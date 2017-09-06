namespace Energy.Interface
{
    /// <summary>
    /// Interface for striping texts from quotes or other surroundings.
    /// </summary>
    public interface IStrip
    {
        string Strip(string text);
    }
}