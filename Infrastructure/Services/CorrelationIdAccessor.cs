using Application.Common.Interfaces.Logging;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class CorrelationIdAccessor : ICorrelationIdAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CorrelationIdHeader = "X-Correlation-ID";

        public CorrelationIdAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCorrelationId()
        {
            return _httpContextAccessor.HttpContext?.Items[CorrelationIdHeader]?.ToString() ?? string.Empty;
        }
    }
}
