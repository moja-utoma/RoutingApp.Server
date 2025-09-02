using FluentValidation;
using Microsoft.EntityFrameworkCore;
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

// builder.Services.AddSingleton<SoftDeleteInterceptor>();
// builder.Services.AddDbContext<AppDbContext>((sp, options) =>
// {
//     options
//         .UseSqlServer(builder.Configuration.GetConnectionString("RoutingDB"))
//         .AddInterceptors(sp.GetRequiredService<SoftDeleteInterceptor>());
// });

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

//Point

//DeliveryPoint
builder.Services.AddScoped<IDeliveryPointService, DeliveryPointService>();
builder.Services.AddScoped<IPointRepository<DeliveryPoint>, DeliveryPointRepository>();

//Warehouse
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddScoped<IPointRepository<Warehouse>, WarehouseRepository>();

//Route
builder.Services.AddScoped<IRouteService, RouteService>();
builder.Services.AddScoped<IRepository<Route>, Repository<Route>>();

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
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowAngular");

app.MapControllers();

app.Run();
