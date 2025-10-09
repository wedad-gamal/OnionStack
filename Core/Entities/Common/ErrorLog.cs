namespace Core.Entities.Common;
public class ErrorLog : BaseEntity<int>
{
    public string CorrelationId { get; set; }
    public string Message { get; set; } = default!;
    public string? StackTrace { get; set; }
    public string Path { get; set; } = default!;
    public string InnerException { get; set; } = default!;
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

}
