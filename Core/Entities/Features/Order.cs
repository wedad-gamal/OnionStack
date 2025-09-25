using Core.Common;
using Core.Events;

namespace Core.Entities.Features;
public class Order : BaseEntity<Guid>
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }

    public static Order Create(Guid productId, int quantity)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            ProductId = productId,
            Quantity = quantity
        };

        order.AddDomainEvent(new OrderPlacedEvent(order.Id, order.ProductId, order.Quantity));

        return order;
    }
}