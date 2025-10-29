using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoutingApp.Data.Entities;

namespace RoutingApp.Data.Configurations;

public class DeliveryPointConfiguration : IEntityTypeConfiguration<DeliveryPoint>
{
  public void Configure(EntityTypeBuilder<DeliveryPoint> builder)
  {
    new BasePointConfiguration().ConfigureBase(builder);

    builder.ToTable("DeliveryPoints");

    builder.Property(dp => dp.Weight)
      	.HasColumnType("decimal")
      	.HasPrecision(6, 2);
  }
}
