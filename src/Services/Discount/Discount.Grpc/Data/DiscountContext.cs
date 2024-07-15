using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data;

public class DiscountContext: DbContext
{
	public DbSet<Coupon> Coupons { get; set; } = default!;

    public DiscountContext(DbContextOptions<DiscountContext> opt): base(opt)
    {        
    }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Coupon>().HasData(
			new Coupon { Id = 1, Amount= 10, Description = "White and samll IPhone", ProductName = "IPhone 8" },
			new Coupon { Id = 2, Amount = 10, Description = "Blask and large Samsung", ProductName = "Samsung 10" }
			);
	}
}
