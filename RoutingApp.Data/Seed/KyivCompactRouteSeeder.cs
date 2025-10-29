using RoutingApp.Data.Entities;
using Route = RoutingApp.Data.Entities.Route;

namespace RoutingApp.Data.Seed
{

    public static class KyivCompactRouteSeeder
    {
        public static (Warehouse warehouse, Vehicle vehicle, List<DeliveryPoint> deliveryPoints, Route route) GetSeed()
        {
            var warehouse = new Warehouse
            {
                Name = "Kyiv Central Hub",
                Address = "Lvivska Square, Kyiv",
                Latitude = 50.4547m,
                Longitude = 30.5138m,
                Vehicles = new List<Vehicle>()
            };

            var vehicle = new Vehicle
            {
                Name = "Van #A1",
                Capacity = 300,
                Warehouse = warehouse
            };
            warehouse.Vehicles.Add(vehicle);

            var deliveryPoints = new List<DeliveryPoint>
            {
                new DeliveryPoint { Name = "Delivery A", Address = "Yaroslaviv Val St", Latitude = 50.4530m, Longitude = 30.5080m, Weight = 10 },
                new DeliveryPoint { Name = "Delivery B", Address = "Reytarska St", Latitude = 50.4555m, Longitude = 30.5110m, Weight = 8 },
                new DeliveryPoint { Name = "Delivery C", Address = "Zoloti Vorota", Latitude = 50.4489m, Longitude = 30.5133m, Weight = 12 },
                new DeliveryPoint { Name = "Delivery D", Address = "Sichovykh Striltsiv St", Latitude = 50.4572m, Longitude = 30.5145m, Weight = 9 },
                new DeliveryPoint { Name = "Delivery E", Address = "Honchara St", Latitude = 50.4525m, Longitude = 30.5190m, Weight = 11 }
            };

            var route = new Route
            {
                Name = "Kyiv Compact Route",
                Warehouses = new List<Warehouse> { warehouse },
                DeliveryPoints = deliveryPoints
            };

            return (warehouse, vehicle, deliveryPoints, route);
        }

        public static void Seed(AppDbContext context)
        {
            var (warehouse, vehicle, deliveryPoints, route) = GetSeed();
            if (!context.Routes.Any(r => r.Name == route.Name))
            {
                context.Warehouses.Add(warehouse);
                context.Vehicles.Add(vehicle);
                context.DeliveryPoints.AddRange(deliveryPoints);
                context.Routes.Add(route);

                context.SaveChanges();
            }
        }
    }
}
