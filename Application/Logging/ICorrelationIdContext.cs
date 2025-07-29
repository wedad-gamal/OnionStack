
namespace Application.Logging
{
    public interface ICorrelationIdContext
    {
        string CorrelationId { get; set; }
    }
}
