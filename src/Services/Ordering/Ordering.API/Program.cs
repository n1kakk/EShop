using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container
builder.Services.AddApplicationServices()
	.AddInfrastractureServices(builder.Configuration)
	.AddApiServices();


var app = builder.Build();


//Configure the Http request pipeline

app.UseApiServices();

if (app.Environment.IsDevelopment())
{
	await app.InitialiseDatabaseAsync();
}

app.Run();
