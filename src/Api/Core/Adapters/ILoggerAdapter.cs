namespace Core.Adapters
{
    public interface ILoggerAdapter<T>
    {
        void Information(string message);
        void Warning(string message);
        void Error(string message);
    }
}
