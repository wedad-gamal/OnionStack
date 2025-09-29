namespace Abstraction.Abstraction.Models
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string? CorrelationId { get; set; }
    }
}
