using Basket.API.Basket.GetBasket;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Basket.API.Basket.DeleteBasket;

public record DeleteBasketResponse(bool IsSuccess);

public class DeleteBasketEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapDelete("/basket/{userName}", async (string userName, ISender sender) =>
		{
			var res = await sender.Send(new DeleteBasketCommand(userName));
			var response = res.Adapt<DeleteBasketResponse>();
			return Results.Ok(response);
		}).WithName("DeleteProduct")
		  .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
		  .ProducesProblem(StatusCodes.Status400BadRequest)
		  .WithSummary("Delete Product")
		  .WithDescription("Delete Product");
	}
}
