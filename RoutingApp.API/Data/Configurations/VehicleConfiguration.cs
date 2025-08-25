using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Hosting;
using RoutingApp.API.Data.Entities;
using Route = RoutingApp.API.Data.Entities.Route;

namespace RoutingApp.API.Data.Configurations
{
	public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
	{
		public void Configure(EntityTypeBuilder<Vehicle> builder)
		{
			builder.ToTable("Vehicles");

			builder.HasQueryFilter(r => !r.IsDeleted);

			builder.HasKey(p => p.Id);

			builder.Property(p => p.Name)
				.HasMaxLength(150)
				.IsRequired();

			builder.Property(p => p.Capacity)
				.HasColumnType("decimal")
				.HasPrecision(6, 2);

			builder.HasOne(b => b.Warehouse).WithMany(b => b.Vehicles);

			builder.Navigation(p => p.Warehouse).AutoInclude();
		}
	}
}
