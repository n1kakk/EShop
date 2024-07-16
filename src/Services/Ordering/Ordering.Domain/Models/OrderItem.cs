
using Ordering.Domain.Abstractions;
using Ordering.Domain.ValueObjects;

namespace Ordering.Domain.Models;

public class OrderItem : Entity<OrderItemId>
{
    internal OrderItem(OrderId oredrId, ProductId productId, int quantity, decimal price)
    {
        Id = OrderItemId.Of(Guid.NewGuid());
        OrderId = oredrId;
        ProductId = productId;
        Quantity = quantity;
        Price = price;
    }

    public OrderId OrderId { get; private set; } = default!;
	public ProductId ProductId { get; private set; } = default!;
	public int Quantity { get; private set; } = default!;
	public decimal Price { get; private set; } = default!;
}
