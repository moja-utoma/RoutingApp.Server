using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoutingApp.Data.Entities;
using System.Reflection.Emit;

namespace RoutingApp.Data.Configurations
{
	public class BasePointConfiguration
	{
		public void ConfigureBase<TEntity>(EntityTypeBuilder<TEntity> builder) where TEntity : Point
		{
			//builder.ToTable("Points");

			builder.HasQueryFilter(r => !r.IsDeleted);

			builder.HasKey(p => p.Id);

			builder.Property(p => p.Name)
				.HasMaxLength(50)
				.IsRequired();

			builder.Property(p => p.Address)
				.HasMaxLength(200)
				.IsRequired();

			builder.Property(p => p.Longitude)
				.HasColumnType("decimal")
				.HasPrecision(17, 14);

			builder.Property(p => p.Latitude)
                .HasColumnType("decimal")
				.HasPrecision(17, 14);
		}
	}
}
