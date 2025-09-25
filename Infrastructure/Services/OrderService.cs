using Core.Entities.Features;

namespace Infrastructure.Services;
public class OrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task PlaceOrderAsync(Order order)
    {
        var productRepo = _unitOfWork.Repository<Product, Guid>();
        var orderRepo = _unitOfWork.Repository<Order, Guid>();

        var product = await productRepo.GetByIdAsync(order.ProductId);

        if (product == null || product.Stock < order.Quantity)
            throw new Exception("Not enough stock");

        product.Stock -= order.Quantity;
        await orderRepo.AddAsync(order);
        productRepo.Update(product);

        await _unitOfWork.SaveChangesAsync();
    }
}

