using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container

builder.Services.AddReverseProxy()
	.LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddRateLimiter(rateLimiterOpts =>
{
	rateLimiterOpts.AddFixedWindowLimiter("fixed", opts =>
	{
		opts.Window = TimeSpan.FromSeconds(10);
		opts.PermitLimit = 5;
	});
});

var app = builder.Build();


//configure the http request pipeline
app.UseRateLimiter();
app.MapReverseProxy();

app.Run();
