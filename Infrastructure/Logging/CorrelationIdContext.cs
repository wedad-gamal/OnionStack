namespace Shared.Logging
{
    public class CorrelationIdContext : ICorrelationIdContext
    {
        private static readonly AsyncLocal<string> _correlationId = new();

        public string CorrelationId
        {
            get => _correlationId.Value ?? Guid.NewGuid().ToString();
            set => _correlationId.Value = value;
        }

    }
}
