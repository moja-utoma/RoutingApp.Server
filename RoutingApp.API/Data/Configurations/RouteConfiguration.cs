using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Hosting;
using RoutingApp.API.Data.Entities;
using Route = RoutingApp.API.Data.Entities.Route;

namespace RoutingApp.API.Data.Configurations
{
	public class RouteConfiguration : IEntityTypeConfiguration<Route>
	{
		public void Configure(EntityTypeBuilder<Route> builder)
		{
			builder.ToTable("Routes");

			builder.HasQueryFilter(r => !r.IsDeleted);

			builder.HasKey(p => p.Id);

			builder.Property(p => p.Name)
				.HasMaxLength(150)
				.IsRequired();

			builder.HasMany(b => b.Warehouses).WithMany(b => b.Routes);
			builder.HasMany(b => b.DeliveryPoints).WithMany(b => b.Routes);

			builder.Navigation(p => p.Warehouses).AutoInclude();
			builder.Navigation(p => p.DeliveryPoints).AutoInclude();
            //builder.Navigation(p => p.CalculatedRoutes).AutoInclude();
        }
	}
}
