
using CatalogAPI.Products.CreateProduct;

namespace CatalogAPI.Products.GetProducts;

public record GetProductsResponse(IEnumerable<Product> Products);
public class GetProductEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapGet("/products", async (ISender sender) =>
		{
			var res = await sender.Send(new GetProductQuery());
			var response = res.Adapt<GetProductsResponse>();
			return Results.Ok(response);	
		}).WithName("GetProducts")
		  .Produces<GetProductsResponse>(StatusCodes.Status200OK)
		  .ProducesProblem(StatusCodes.Status400BadRequest)
		  .WithSummary("Get Products")
		  .WithDescription("Get Products"); 
	}
}
