using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Infrastructure.Data;

namespace Ordering.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastractureServices(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("Database");

		//Add Services to the container
		services.AddDbContext<ApplicationDbContext>(opts =>
		{
			opts.UseSqlServer(connectionString);
		});

		return services;
	}
}
