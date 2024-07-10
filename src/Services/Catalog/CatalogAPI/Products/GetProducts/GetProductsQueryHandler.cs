
namespace CatalogAPI.Products.GetProducts;

public record GetProductQuery() : IQuery<GetProductsResult>;
public record GetProductsResult(IEnumerable<Product> Products);
internal class GetProductsQueryHandler (IDocumentSession session, ILogger<GetProductsQueryHandler> logger)
	: IQueryHandler<GetProductQuery, GetProductsResult>
{
	public async Task<GetProductsResult> Handle(GetProductQuery query, CancellationToken cancellationToken)
	{
		logger.LogInformation("GetProductsQueryHandler. Handle called with {@Query}", query);
		var products = await session.Query<Product>().ToListAsync(cancellationToken);
		return new GetProductsResult(products);
	}
}
