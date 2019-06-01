namespace Energy.Interface
{
    public interface IWorker: IWork, IStop, IStopped, IRunning, IAbort
    {
    }
}