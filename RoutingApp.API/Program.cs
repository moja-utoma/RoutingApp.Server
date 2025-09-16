using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using RoutingApp.API.Data;
using RoutingApp.API.Data.Entities;
using RoutingApp.API.Data.Interceptors;
using RoutingApp.API.Models.DTO;
using RoutingApp.API.Repositories;
using RoutingApp.API.Repositories.Interfaces;
using RoutingApp.API.Services;
using RoutingApp.API.Services.Interfaces;
using RoutingApp.API.Validation;
using Route = RoutingApp.API.Data.Entities.Route;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddHttpClient();

builder.Services.AddDbContext<AppDbContext>(options =>
    options
        .UseSqlServer(builder.Configuration.GetConnectionString("RoutingDB")
        //, sqlServerOptionsAction: sqlOptions =>
        //{
        //    sqlOptions.EnableRetryOnFailure(
        //        maxRetryCount: 3,
        //        maxRetryDelay: TimeSpan.FromSeconds(30),
        //        errorNumbersToAdd: null);
        //}
        )
        .AddInterceptors(new SoftDeleteInterceptor())
        );

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

//DeliveryPoint
builder.Services.AddScoped<IDeliveryPointService, DeliveryPointService>();
builder.Services.AddScoped<IPointRepository<DeliveryPoint>, DeliveryPointRepository>();

//Warehouse
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddScoped<IPointRepository<Warehouse>, WarehouseRepository>();

//Route
builder.Services.AddScoped<IRouteService, RouteService>();
builder.Services.AddScoped<IRepository<Route>, Repository<Route>>();
builder.Services.AddScoped<IRouteRepository, RouteRepository>();

//Vehicle
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IRepository<Vehicle>, Repository<Vehicle>>();

//Validation
builder.Services.AddScoped<IValidator<CreateRouteRequestDTO>, RouteValidator>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

var app = builder.Build();

app.UseCors("AllowAngular");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.Use(async (context, next) =>
//{
//    if (context.Request.Method == HttpMethods.Options)
//    {
//        context.Response.StatusCode = StatusCodes.Status204NoContent;
//        context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
//        context.Response.Headers.Append("Access-Control-Allow-Headers", "Authorization, Content-Type, Accept");
//        context.Response.Headers.Append("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
//        return;
//    }

//    await next();
//});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
