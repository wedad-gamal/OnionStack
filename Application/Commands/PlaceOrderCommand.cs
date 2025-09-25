namespace Application.Commands;
public record PlaceOrderCommand(Guid ProductId, int Quantity) : IRequest<Guid>;
