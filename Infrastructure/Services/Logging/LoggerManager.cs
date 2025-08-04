namespace Infrastructure.Services.Logging
{
    public class LoggerManager : ILoggerManager
    {
        private readonly ILogger _logger;
        private readonly ICorrelationIdAccessor _correlation;

        public LoggerManager(ICorrelationIdAccessor correlationId)
        {
            //_logger = Log.ForContext("SourceContext", "Application");
            _correlation = correlationId;
            _logger = new LoggerConfiguration()
                           .Enrich.FromLogContext()
                           .WriteTo.Console()
                           .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                           .CreateLogger();
        }
        public void Info(string message, params object[] args)
        {
            var correlationId = Guid.NewGuid().ToString();
            correlationId = _correlation.GetCorrelationId();

            _logger.ForContext("CorrelationId", correlationId).Information(message, args);
        }

        public void Warn(string message, params object[] args) =>
            _logger.Warning(message, args);

        public void Error(string message, params object[] args) =>
            _logger.Error(message, args);

        public void Debug(string message, params object[] args) =>
            _logger.Debug(message, args);
    }
}
