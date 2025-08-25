using RoutingApp.API.Data.Entities;
using RoutingApp.API.Models.DTO;
using RoutingApp.API.Models.Response;
using Route = RoutingApp.API.Data.Entities.Route;

namespace RoutingApp.API.Mappers
{
	public static class EntityToModel
	{
		public static DeliveryPointResponse CreateModelFromDeliveryPoint(DeliveryPoint point)
		{
			return new DeliveryPointResponse
			{
				Id = point.Id,
				Name = point.Name,
				Address = point.Address,
				Longitude = point.Longitude,
				Latitude = point.Latitude,
				Weight = point.Weight,
			};
		}

		public static IEnumerable<DeliveryPointResponse> CreateModelsFromDeliveryPoints(IEnumerable<DeliveryPoint> points)
		{
			return points.Select(CreateModelFromDeliveryPoint);
		}

		public static WarehouseResponse CreateModelFromWarehouse(Warehouse point)
		{
			return new WarehouseResponse
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

		public static IEnumerable<WarehouseResponse> CreateModelsFromWarehouses(IEnumerable<Warehouse> points)
		{
			return points.Select(CreateModelFromWarehouse);
		}

		public static RouteResponse CreateModelFromRoute(Route route)
		{
			return new RouteResponse
			{
				Id = route.Id,
				Name = route.Name,
				Warehouses = CreateModelsFromWarehouses(route.Warehouses),
				DeliveryPoints = CreateModelsFromDeliveryPoints(route.DeliveryPoints)
			};
		}

		public static IEnumerable<RouteResponse> CreateModelsFromRoutes(IEnumerable<Route> routes)
		{
			return routes.Select(CreateModelFromRoute);
		}

		public static VehicleResponse CreateModelFromVehicle(Vehicle vehicle)
		{
			return new VehicleResponse
			{
				Id = vehicle.Id,
				Name = vehicle.Name,
				Capacity = vehicle.Capacity,
				Warehouse= vehicle.Warehouse != null ?
				CreateModelFromWarehouse(vehicle.Warehouse) : null
			};
		}

		public static IEnumerable<VehicleResponse> CreateModelsFromVehicles(IEnumerable<Vehicle> vehicles)
		{
			return vehicles.Select(CreateModelFromVehicle);
		}
	}
}
