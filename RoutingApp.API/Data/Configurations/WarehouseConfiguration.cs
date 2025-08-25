using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoutingApp.API.Data.Entities;

namespace RoutingApp.API.Data.Configurations;

public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
  public void Configure(EntityTypeBuilder<Warehouse> builder)
  {
    new BasePointConfiguration().ConfigureBase(builder);

    builder.ToTable("Warehouses");

    builder.Navigation(p => p.Vehicles).AutoInclude();
  }
}
