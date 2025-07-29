namespace Infrastructure.Logging
{
    public class SerilogLoggerService : ILoggerService
    {
        private readonly ILogger _logger;
        private readonly ICorrelationIdContext _correlationIdContext;

        public SerilogLoggerService(ICorrelationIdContext correlationIdContext)
        {
            _logger = Log.ForContext("SourceContext", "Application");
            _correlationIdContext = correlationIdContext;
        }
        public void Info(string message, params object[] args)
        {
            var correlationId = Guid.NewGuid().ToString();
            correlationId = _correlationIdContext.CorrelationId ?? correlationId;

            _logger.ForContext("CorrelationId", correlationId).Information(message, args);
        }

        public void Warn(string message, params object[] args) =>
            _logger.Warning(message, args);

        public void Error(string message, Exception ex = null, params object[] args) =>
            _logger.Error(ex, message, args);

        public void Debug(string message, params object[] args) =>
            _logger.Debug(message, args);
    }
}
