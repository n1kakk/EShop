using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Configuration;

namespace Discount.Grpc.Services;

public class DiscountService (DiscountContext discountContext, ILogger<DiscountService> logger)
	: DiscountProtoService.DiscountProtoServiceBase
{
	public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
	{
		var coupon = await discountContext
			.Coupons
			.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

        if (coupon == null)
        {
            coupon = new Coupon { ProductName = "No discount", Amount = 0, Description = "No discount description" };
        }

		logger.LogInformation("Discount is received for ProductName : {productName}, Amount: {amount}", coupon.ProductName, coupon.Amount);

		var couponModel = coupon.Adapt<CouponModel>();

		return couponModel;
    }

	public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
	{
		var coupon = request.Coupon.Adapt<Coupon>();
		if (coupon is null)
			throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));

		discountContext.Coupons.Add(coupon);
		await discountContext.SaveChangesAsync();

		logger.LogInformation("Discount is successfully created. Product name: {productName}", coupon.ProductName);

		var couponModel = coupon.Adapt<CouponModel>();
		return couponModel;

	}

	public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
	{
		var coupon = request.Coupon.Adapt<Coupon>();
		if (coupon is null)
			throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));

		discountContext.Coupons.Update(coupon);
		await discountContext.SaveChangesAsync();

		logger.LogInformation("Discount is successfully updated. Product name: {productName}", coupon.ProductName);

		var couponModel = coupon.Adapt<CouponModel>();
		return couponModel;
	}

	public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
	{
		var coupon = await discountContext
		.Coupons
		.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

		if (coupon is null)
			throw new RpcException(new Status(StatusCode.NotFound, "Discount is not found"));

		discountContext.Coupons.Remove(coupon);
		await discountContext.SaveChangesAsync();

		logger.LogInformation("Discount is successfully deleted. Product name: {productName}", coupon.ProductName);

		return new DeleteDiscountResponse { Success = true };

	}
}
