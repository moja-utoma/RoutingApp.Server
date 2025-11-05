using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RoutingApp.Data;
using RoutingApp.Data.Repositories;
using RoutingApp.Data.Repositories.Interfaces;

var host = new HostBuilder()
    .ConfigureAppConfiguration(config =>
    {
        config.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);
        config.AddEnvironmentVariables();
    })
    .ConfigureServices((context, services) =>
    {
        var sqlConnection = context.Configuration["Sql:ConnectionString"];
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(sqlConnection));

		var connectionString = context.Configuration["ServiceBus:ConnectionString"];
		services.AddSingleton(new ServiceBusClient(connectionString));

		services.AddScoped<IRouteRepository, RouteRepository>();
    })
    .ConfigureFunctionsWorkerDefaults()
    .Build();

host.Run();
