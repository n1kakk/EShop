using Basket.API.Basket.SetBasket;
using Basket.API.Data;
using Basket.API.Models;
using BuildingBlocks.CQRS;
using FluentValidation;

namespace Basket.API.Basket.DeleteBasket;

public record DeleteBasketCommand(string UserName) : ICommand<DeleteBasketResult>;

public record DeleteBasketResult(bool IsSuccess);


public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
	public DeleteBasketCommandValidator()
	{
		RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is required");
	}
}
public class DeleteBasketHandler(IBasketRepository repo) : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
	public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
	{
		
		repo.DeleteBasket(command.UserName, cancellationToken);

		return new DeleteBasketResult(true);
	}
}
