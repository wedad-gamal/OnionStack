

namespace Abstraction.Abstraction.Interfaces.Logging;
public interface IErrorLogService
{
    Task LogAsync(Exception ex, HttpContext context, string correlationId = "");
}