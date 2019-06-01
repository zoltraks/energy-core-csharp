namespace Energy.Interface
{
    public interface IWorker: IWork, IStart, IStop, IStopped, IRunning, IAbort
    {
    }
}