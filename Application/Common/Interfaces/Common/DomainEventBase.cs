namespace Application.Common.Interfaces.Common;
// Adapter base class so your domain events can also be MediatR notifications
public abstract record DomainEventBase : IDomainEvent, INotification;