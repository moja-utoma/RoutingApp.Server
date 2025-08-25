using RoutingApp.API.Data.Entities;
using RoutingApp.API.Enumerations;
using RoutingApp.API.Models;
using RoutingApp.API.Models.DTO;
using RoutingApp.API.Models.Request;
using Route = RoutingApp.API.Data.Entities.Route;

namespace RoutingApp.API.Mappers
{
	public class ModelToEntity
	{
		public static DeliveryPoint CreateEntityFromDeliveryPoint(CreateDeliveryPointRequestDTO point)
		{
			DeliveryPoint entity = new DeliveryPoint
			{
				Name = point.Name,
				Address = point.Address,
				Longitude = point.Longitude,
				Latitude = point.Latitude,
				Weight = point.Weight,
			};

			return entity;
		}

		public static Warehouse CreateEntityFromWarehouse(CreatePointRequestDTO point)
		{
			Warehouse entity = new Warehouse
			{
				Name = point.Name,
				Address = point.Address,
				Longitude = point.Longitude,
				Latitude = point.Latitude
			};

			return entity;
		}

		public static Route CreateEntityFromRoute(CreateRouteRequestDTO route, IEnumerable<DeliveryPoint> deliveryPoints, IEnumerable<Warehouse> warehouses)
		{
			Route entity = new Route
			{
				Name = route.Name,
				DeliveryPoints = deliveryPoints,
				Warehouses = warehouses
			};
			return entity;
		}

		public static Vehicle CreateEntityFromVehicle(CreateVehicleRequestDTO vehicle, Warehouse? warehouse = null)
		{
			Vehicle entity = new Vehicle
			{
				Name = vehicle.Name,
				Capacity = vehicle.Capacity,
				Warehouse = warehouse
			};
			return entity;
		}
		
		
	}
}
