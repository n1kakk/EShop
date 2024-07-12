using Basket.API.Data;
using Basket.API.Models;
using BuildingBlocks.CQRS;
using FluentValidation;

namespace Basket.API.Basket.SetBasket;

public record StoreBasketCommand(ShoppingCart Cart): ICommand<StoreBasketResult>;

public record StoreBasketResult(string UserName);

public class StoreBasketCommandValidator: AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart).NotNull().WithMessage("Shopping cart can not be null");
		RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("Username is required");
	}
}

public class StoreBasketCommandHandler(IBasketRepository repo) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
	public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
	{
		ShoppingCart cart = command.Cart;

		await repo.SroteBasket(cart, cancellationToken);

		//TODO: update cache

		return new StoreBasketResult(command.Cart.UserName);
	}
}
