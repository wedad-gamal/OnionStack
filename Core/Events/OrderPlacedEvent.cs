namespace Core.Events;
public record OrderPlacedEvent(Guid OrderId, Guid ProductId, int Quantity) : IDomainEvent;