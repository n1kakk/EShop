using Basket.API.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Data;

public class CachedBasketRepo(IBasketRepository repo, IDistributedCache distributedCache) : IBasketRepository
{
	public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
	{
		await repo.DeleteBasket(userName, cancellationToken);
		await distributedCache.RemoveAsync(userName, cancellationToken);
		return true;
	}

	public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
	{
		var cachedBasket = await distributedCache.GetStringAsync(userName, cancellationToken);

		if(!string.IsNullOrEmpty(cachedBasket)) {return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!; }

		var basket = await repo.GetBasket(userName, cancellationToken);
		await distributedCache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);

		return basket;
	}

	public async Task<ShoppingCart> SroteBasket(ShoppingCart cart, CancellationToken cancellationToken = default)
	{
		await repo.SroteBasket(cart, cancellationToken);
		await distributedCache.SetStringAsync(cart.UserName, JsonSerializer.Serialize(cart), cancellationToken);
		return cart;
	}
}
