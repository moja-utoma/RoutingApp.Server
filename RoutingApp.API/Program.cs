using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Azure.Security.KeyVault.Secrets;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using RoutingApp.API.Models.DTO;
using RoutingApp.API.Services;
using RoutingApp.API.Services.Interfaces;
using RoutingApp.API.Validation;
using RoutingApp.Data;
using RoutingApp.Data.Entities;
using RoutingApp.Data.Interceptors;
using RoutingApp.Data.Repositories;
using RoutingApp.Data.Repositories.Interfaces;
using RoutingApp.Data.Seed;
using Route = RoutingApp.Data.Entities.Route;

var builder = WebApplication.CreateBuilder(args);

var keyVaultUrl = builder.Configuration["AzureKeyVault:VaultUri"];

if (!string.IsNullOrEmpty(keyVaultUrl))
{
    var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());

    try
    {
        await foreach (var secretProperties in client.GetPropertiesOfSecretsAsync())
        {
            try
            {
                var secret = await client.GetSecretAsync(secretProperties.Name);
                builder.Configuration[secretProperties.Name] = secret.Value.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to retrieve secret '{secretProperties.Name}': {ex.Message}");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Key Vault access failed: {ex.Message}");
    }
}

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
		.UseSqlServer(
			builder.Configuration["RoutingDB"],
			sql => sql.MigrationsAssembly("RoutingApp.Data")
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

builder.Services.AddScoped<ICalculatedRouteRepository, CalculatedRouteRepository>();

//Vehicle
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IRepository<Vehicle>, Repository<Vehicle>>();

//Validation
builder.Services.AddScoped<IValidator<CreateRouteRequestDTO>, RouteValidator>();

//Ors
builder.Services.AddScoped<IOrsService, OrsService>();

builder.Services.AddScoped<IFileStorageService, AzureBlobStorageService>();
builder.Services.AddScoped<IRepository<FileRecord>, Repository<FileRecord>>();

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

builder.Services.AddSignalR();

builder.Services.AddHostedService<RouteReplyListener>();
builder.Services.AddApplicationInsightsTelemetry(new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions
{
	ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]
});

builder.Services.AddApplicationInsightsTelemetry();

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

app.MapHub<RouteHub>("/routeHub");

app.Run();
