using Serilog.Core;
using Serilog.Events;

namespace Infrastructure.Logging
{
    public class CorrelationIdEnricher : ILogEventEnricher
    {
        private readonly ICorrelationIdContext _context;

        public CorrelationIdEnricher(ICorrelationIdContext context)
        {
            _context = context;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var correlationId = _context?.CorrelationId ?? "N/A";
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("CorrelationId", correlationId));
        }
    }
}
