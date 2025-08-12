namespace Infrastructure.Services.Logging
{
    public class LoggerManager : ILoggerManager
    {
        private readonly ICorrelationContextAccessor _correlation;
        private readonly ILogger<LoggerManager> _logger;

        public LoggerManager(ICorrelationContextAccessor correlation, ILogger<LoggerManager> logger)
        {
            _correlation = correlation;
            _logger = logger;
        }

        private string CorrelationId => _correlation.CorrelationContext?.CorrelationId ?? "N/A";

        private object[] PrependCorrelation(object[] args) =>
            new object[] { CorrelationId }.Concat(args ?? new object[0]).ToArray();

        public void Info(string message, params object[] args) =>
            _logger.LogInformation("[{CorrelationId}] " + message, PrependCorrelation(args));

        public void Warn(string message, params object[] args) =>
            _logger.LogWarning("[{CorrelationId}] " + message, PrependCorrelation(args));

        public void Error(string message, params object[] args) =>
            _logger.LogError("[{CorrelationId}] " + message, PrependCorrelation(args));

        public void Debug(string message, params object[] args) =>
            _logger.LogDebug("[{CorrelationId}] " + message, PrependCorrelation(args));
    }
}
