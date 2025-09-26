using Microsoft.EntityFrameworkCore;
using RoutingApp.API.Data.Entities;
using Route = RoutingApp.API.Data.Entities.Route;

namespace RoutingApp.API.Data.Seed
{
    public static class RouteSeeder
    {
        public static (Warehouse warehouse, Vehicle vehicle, List<DeliveryPoint> deliveryPoints, Route route) GetSeed()
        {
            var warehouse = new Warehouse
            {
                Name = "Main Kyiv Warehouse",
                Address = "Shevchenko Blvd, Kyiv",
                Latitude = 50.4501m,
                Longitude = 30.5234m,
                Vehicles = new List<Vehicle>()
            };

            var vehicle = new Vehicle
            {
                Name = "Truck #1",
                Capacity = 500,
                Warehouse = warehouse
            };
            warehouse.Vehicles.Add(vehicle);

            var deliveryPoints = new List<DeliveryPoint>
            {
                new DeliveryPoint { Name = "Delivery #1", Address = "Khreshchatyk St", Latitude = 50.4470m, Longitude = 30.5220m, Weight = 20 },
                new DeliveryPoint { Name = "Delivery #2", Address = "Peremohy Ave", Latitude = 50.4547m, Longitude = 30.4480m, Weight = 15 },
                new DeliveryPoint { Name = "Delivery #3", Address = "Volodymyrska St", Latitude = 50.4454m, Longitude = 30.5114m, Weight = 12 },
                new DeliveryPoint { Name = "Delivery #4", Address = "Hoholivska St", Latitude = 50.4540m, Longitude = 30.4890m, Weight = 18 },
                new DeliveryPoint { Name = "Delivery #5", Address = "Dorohozhytska St", Latitude = 50.4712m, Longitude = 30.4625m, Weight = 22 },
                new DeliveryPoint { Name = "Delivery #6", Address = "Naberezhne Hwy", Latitude = 50.4459m, Longitude = 30.5439m, Weight = 30 },
                new DeliveryPoint { Name = "Delivery #7", Address = "Borshchahivska St", Latitude = 50.4543m, Longitude = 30.4225m, Weight = 25 },
                new DeliveryPoint { Name = "Delivery #8", Address = "Velyka Vasylkivska St", Latitude = 50.4375m, Longitude = 30.5160m, Weight = 10 },
                new DeliveryPoint { Name = "Delivery #9", Address = "Syretska St", Latitude = 50.4790m, Longitude = 30.4542m, Weight = 16 },
                new DeliveryPoint { Name = "Delivery #10", Address = "Oleny Teligy St", Latitude = 50.4795m, Longitude = 30.4628m, Weight = 14 }
            };

            var route = new Route
            {
                Name = "Kyiv Route #1",
                Warehouses = new List<Warehouse> { warehouse },
                DeliveryPoints = deliveryPoints
            };

            return (warehouse, vehicle, deliveryPoints, route);
        }


        public static void Seed(AppDbContext context)
        {
                var (warehouse, vehicle, deliveryPoints, route) = GetSeed();
            if ( !context.Routes.Any(r =>  r.Name == route.Name))
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
