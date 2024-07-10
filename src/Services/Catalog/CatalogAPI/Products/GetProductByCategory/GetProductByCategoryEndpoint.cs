
using CatalogAPI.Products.GetProductById;
using System.Collections;

namespace CatalogAPI.Products.GetProductByCategory;

public record GetProductByCategoryResponse(IEnumerable<Product> Products);

public class GetProductByCategoryEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapGet("/products/sategory/{category}", async(string category, ISender sender) =>
		{
			var res = await sender.Send(new GetProductByCategoryQuery(category));
			var response = res.Adapt<GetProductByCategoryResponse>();
			return Results.Ok(response);
		}).WithName("GetProductByCategory")
		  .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
		  .ProducesProblem(StatusCodes.Status400BadRequest)
		  .WithSummary("Get Product by Category")
		  .WithDescription("Get Product by Category");
	}
}
