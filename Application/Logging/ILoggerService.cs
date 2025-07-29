﻿namespace Application.Logging
{
    public interface ILoggerService
    {
        void Info(string message, params object[] args);
        void Warn(string message, params object[] args);
        void Error(string message, Exception ex = null, params object[] args);
        void Debug(string message, params object[] args);
    }
}
