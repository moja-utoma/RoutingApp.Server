using RoutingApp.API.Data.Entities;
using RoutingApp.API.Models.Response.GetAll;
using RoutingApp.API.Models.Response.GetByID;
using Route = RoutingApp.API.Data.Entities.Route;

namespace RoutingApp.API.Mappers
{
	public static class EntityToModel
	{
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

        public static VehiclesForWarehouseDTO CreateModelFromVehicleForWarehouse(Vehicle vehicle)
        {
            return new VehiclesForWarehouseDTO
            {
                Id = vehicle.Id,
                Name = vehicle.Name,
                Capacity = vehicle.Capacity
            };
        }

        public static DetailsWarehouseResponseDTO CreateOneModelFromWarehouse(Warehouse point)
        {
            return new DetailsWarehouseResponseDTO
            {
                Id = point.Id,
                Name = point.Name,
                Address = point.Address,
                Longitude = point.Longitude,
                Latitude = point.Latitude,
                Vehicles = point.Vehicles != null ? point.Vehicles.Select(CreateModelFromVehicleForWarehouse).ToList() : null,
            };
        }

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
	}
}
