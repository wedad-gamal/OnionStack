namespace Application.Common.Interfaces.Logging
{
    public interface ICorrelationIdAccessor
    {
        string GetCorrelationId();
    }
}
