namespace Core.Interfaces
{
    public interface ILoggerManager 
    {
        void Info(string message, params object[] args);
        void Warn(string message, params object[] args);
        void Error(string message, params object[] args);
        void Debug(string message, params object[] args);
    }
}
