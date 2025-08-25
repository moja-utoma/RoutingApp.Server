using CsvHelper.Configuration;
using RoutingApp.API.Data.Entities;

public sealed class WarehouseMap : ClassMap<Warehouse>
{
    public WarehouseMap()
    {
        Map(p => p.Name);
        Map(p => p.Address);
        Map(p => p.Longitude);
        Map(p => p.Latitude);
    }
}

public sealed class DeliveryPointMap : ClassMap<DeliveryPoint>
{
    public DeliveryPointMap()
    {
        Map(p => p.Name);
        Map(p => p.Address);
        Map(p => p.Longitude);
        Map(p => p.Latitude);
        Map(p => p.Weight);
    }
}