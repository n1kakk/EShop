using Basket.API.Models;

namespace Basket.API.Data;

public interface IBasketRepository
{
	Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default);
	Task<ShoppingCart> SroteBasket(ShoppingCart cart, CancellationToken cancellationToken = default);
	Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken= default);

}
