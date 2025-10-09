using Core.Entities;

namespace Application.Handlers;
public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public PlaceOrderCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        var productRepo = _unitOfWork.Repository<Product, Guid>();
        var orderRepo = _unitOfWork.Repository<Order, Guid>();

        var product = await productRepo.GetByIdAsync(request.ProductId);
        if (product == null || product.Stock < request.Quantity)
            throw new Exception("Not enough stock");

        product.Stock -= request.Quantity;
        var order = Order.Create(request.ProductId, request.Quantity);

        await orderRepo.AddAsync(order);
        productRepo.Update(product);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return order.Id;
    }
}