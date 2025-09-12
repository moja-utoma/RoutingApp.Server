using Microsoft.EntityFrameworkCore;
using RoutingApp.API.Data.Configurations;
using RoutingApp.API.Data.Entities;
using RoutingApp.API.Data.Interceptors;
using RoutingApp.API.Models;
using Route = RoutingApp.API.Data.Entities.Route;

namespace RoutingApp.API.Data
{
	public class AppDbContext : DbContext
	{
		public DbSet<Route> Routes { get; set; }
		public DbSet<DeliveryPoint> DeliveryPoints { get; set; }
		public DbSet<Warehouse> Warehouses { get; set; }
		public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<CalculatedRoute> CalculatedRoutes { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new RouteConfiguration());
			modelBuilder.ApplyConfiguration(new DeliveryPointConfiguration());
			modelBuilder.ApplyConfiguration(new WarehouseConfiguration());
			modelBuilder.ApplyConfiguration(new VehicleConfiguration());

			modelBuilder.Entity<CalculatedRoute>(builder =>
			{
				builder.ToTable("CalculatedRoutes");

				builder.HasKey(cr => cr.Id);

				builder.HasOne(r => r.Route).WithMany(cr => cr.CalculatedRoutes);

                builder.Property(p => p.Calculation)
                .HasColumnType("NVARCHAR(MAX)");

				builder.HasQueryFilter(cr => !cr.Route.IsDeleted);
            });
		}
	}
}
