using RoutingApp.API.Data.Entities;
using RoutingApp.API.Models.Responses.DeliveryPoints;
using RoutingApp.API.Models.Responses.Routes;
using RoutingApp.API.Models.Responses.Vehicles;
using RoutingApp.API.Models.Responses.Warehouse;
using RoutingApp.API.Models.Responses.Warehouses;
using Route = RoutingApp.API.Data.Entities.Route;

namespace RoutingApp.API.Mappers
{
    public static class EntityToModel
    {
        // Delivery Points
        public static DeliveryPointResponseDTO CreateModelFromDeliveryPoint(DeliveryPoint point)
        {
            return new DeliveryPointResponseDTO
            {
                Id = point.Id,
                Name = point.Name,
                Address = point.Address,
                Longitude = point.Longitude,
                Latitude = point.Latitude,
                Weight = point.Weight,
            };
        }

        public static IEnumerable<DeliveryPointResponseDTO> CreateModelsFromDeliveryPoints(IEnumerable<DeliveryPoint> points)
        {
            return points.Select(CreateModelFromDeliveryPoint);
        }

        public static DeliveryPointDetailsResponseDTO CreateModelForDetailsFromDeliveryPoint(DeliveryPoint point)
        {
            return new DeliveryPointDetailsResponseDTO
            {
                Id = point.Id,
                Name = point.Name,
                Address = point.Address,
                Latitude = point.Latitude,
                Longitude = point.Longitude,
                Weight = point.Weight,
            };
        }

        // Warehouses

        public static WarehouseResponseDTO CreateModelFromWarehouse(Warehouse point)
        {
            return new WarehouseResponseDTO
            {
                Id = point.Id,
                Name = point.Name,
                Address = point.Address,
                Longitude = point.Longitude,
                Latitude = point.Latitude,
                VehicleQuantity = point.Vehicles != null ?
                point.Vehicles.Count() : 0
            };
        }

        public static IEnumerable<WarehouseResponseDTO> CreateModelsFromWarehouses(IEnumerable<Warehouse> points)
        {
            return points.Select(CreateModelFromWarehouse);
        }

        public static WarehouseDetailsResponseDTO CreateModelForDetailsFromWarehouse(Warehouse point)
        {
            return new WarehouseDetailsResponseDTO
            {
                Id = point.Id,
                Name = point.Name,
                Address = point.Address,
                Longitude = point.Longitude,
                Latitude = point.Latitude,
                Vehicles = point.Vehicles != null ? point.Vehicles.Select(CreateModelFromVehicle).ToList() : null,
            };
        }

        // Routes

        public static RouteResponseDTO CreateModelFromRoute(Route route)
        {
            return new RouteResponseDTO
            {
                Id = route.Id,
                Name = route.Name,
                DeliveryPointsQuantity = route.DeliveryPoints.Count(),
                WarehouseNames = route.Warehouses.Select(w => w.Name)
            };
        }

        public static IEnumerable<RouteResponseDTO> CreateModelsFromRoutes(IEnumerable<Route> routes)
        {
            return routes.Select(CreateModelFromRoute);
        }

        public static RouteDetailsResponseDTO CreateModelForDetailsFromRoute(Route route)
        {
            return new RouteDetailsResponseDTO
            {
                Id = route.Id,
                Name = route.Name,
                Warehouses = route.Warehouses.Select(CreateModelFromWarehouse),
                DeliveryPoints = route.DeliveryPoints.Select(CreateModelFromDeliveryPoint),
            };
        }

        // Vehicles

        public static VehicleResponseDTO CreateModelFromVehicle(Vehicle vehicle)
        {
            return new VehicleResponseDTO
            {
                Id = vehicle.Id,
                Name = vehicle.Name,
                Capacity = vehicle.Capacity,
                WarehouseId = vehicle.Warehouse != null ? vehicle.Warehouse.Id : null,
                WarehouseName = vehicle.Warehouse != null ? vehicle.Warehouse.Name : null,
                WarehouseAddress = vehicle.Warehouse != null ? vehicle.Warehouse.Address : null,
            };
        }

        public static IEnumerable<VehicleResponseDTO> CreateModelsFromVehicles(IEnumerable<Vehicle> vehicles)
        {
            return vehicles.Select(CreateModelFromVehicle);
        }

        public static VehicleDetailsResponseDTO CreateModelForDetailsFromVehicle(Vehicle vehicle)
        {
            return new VehicleDetailsResponseDTO
            {
                Id = vehicle.Id,
                Name = vehicle.Name,
                Capacity = vehicle.Capacity,
                Warehouse = vehicle.Warehouse != null ? CreateModelFromWarehouse(vehicle.Warehouse) : null
            };
        }
    }
}
