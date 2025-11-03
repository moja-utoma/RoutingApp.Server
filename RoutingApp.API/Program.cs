using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using RoutingApp.API.Models.DTO;
using RoutingApp.API.Services;
using RoutingApp.API.Services.Interfaces;
using RoutingApp.API.Validation;
using Azure.Messaging.ServiceBus;
using Route = RoutingApp.Data.Entities.Route;
using RoutingApp.Data;
using RoutingApp.Data.Interceptors;
using RoutingApp.Data.Entities;
using RoutingApp.Data.Repositories.Interfaces;
using RoutingApp.Data.Repositories;
using RoutingApp.Data.Seed;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

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

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.Authority = builder.Configuration.GetValue<string>("auth0:Authority");
//        options.Audience = builder.Configuration.GetValue<string>("auth0:Audience");
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            NameClaimType = "name"
//        };
//    });


//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

//builder.Services.AddAuthorization(options =>
//{
//    options.FallbackPolicy = options.DefaultPolicy;
//});


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

//Ors
builder.Services.AddScoped<IOrsService, OrsService>();

builder.Services.AddScoped<IFileStorageService, AzureBlobStorageService>();

builder.Services.AddSingleton<IRouteStreamRegistry, RouteStreamRegistry>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200", "https://routing-app-ui-win.azurewebsites.net")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});


builder.Services.AddSingleton<ServiceBusClient>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var connString = config["ServiceBus:ConnectionString"];
    return new ServiceBusClient(connString);
});

builder.Services.AddScoped<IQueuePublisherService, QueuePublisherService>();

var app = builder.Build();

app.UseCors("AllowAngular");

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

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

app.MapGet("/", () => Results.Ok("No auth required."))
   .AllowAnonymous();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    RouteSeeder.Seed(context);
    KyivCompactRouteSeeder.Seed(context);
}

app.Run();
