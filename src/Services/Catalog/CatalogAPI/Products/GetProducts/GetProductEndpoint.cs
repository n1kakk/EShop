﻿namespace CatalogAPI.Products.GetProducts;

public record GetProductsRequest(int? PageNumber = 1, int? PageSize = 12);
public record GetProductsResponse(IEnumerable<Product> Products);
public class GetProductEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapGet("/products", async ([AsParameters] GetProductsRequest request, ISender sender) =>
		{
			var query = request.Adapt<GetProductQuery>();

			var res = await sender.Send(query);

			var response = res.Adapt<GetProductsResponse>();

			return Results.Ok(response);
			
		}).WithName("GetProducts")
		  .Produces<GetProductsResponse>(StatusCodes.Status200OK)
		  .ProducesProblem(StatusCodes.Status400BadRequest)
		  .WithSummary("Get Products")
		  .WithDescription("Get Products"); 
	}
}
