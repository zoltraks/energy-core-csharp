namespace Energy.Interface
{
    public interface ICommandProgram
    {
        bool Setup(string[] args);

        bool Initialize(string[] args);

        bool Run(string[] args);
    }
}