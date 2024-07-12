using Basket.API.Basket.GetBasket;
using Basket.API.Models;
using Carter;
using Mapster;
using MediatR;

namespace Basket.API.Basket.SetBasket;

public record StoreBasketRequest(ShoppingCart Cart);
public record StoreBasketResponse(string UserName);

public class StoreBasketEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapPost("/basket", async (StoreBasketRequest request, ISender sender) =>
		{
			var command = request.Adapt<StoreBasketCommand>();

			var res = await sender.Send(command);
			var response =  res.Adapt<StoreBasketResponse>();

			return Results.Created($"/basket/{response.UserName}", response);
		}).WithName("CreateProduct")
		  .Produces<GetBasketResponse>(StatusCodes.Status200OK)
		  .ProducesProblem(StatusCodes.Status400BadRequest)
		  .WithSummary("Create Product")
		  .WithDescription("Create Product");
	}
}
