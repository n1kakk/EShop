
using CatalogAPI.Products.UpdateProduct;

namespace CatalogAPI.Products.DeleteProduct;

public record DeleteProductResponse(bool IsSuccess);

public class DeleteProductEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapDelete("/products/{id}", async (Guid id, ISender sender) =>
		{
			var res = await sender.Send(new DeleteProductCommand(id));
			var response = res.Adapt<DeleteProductResponse>();

			return Results.Ok(response);	
		}).WithName("DeleteProduct")
		  .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
		  .ProducesProblem(StatusCodes.Status400BadRequest)
		  .ProducesProblem(StatusCodes.Status404NotFound)
		  .WithSummary("Delete Product")
		  .WithDescription("Delete Product");
	}
}
