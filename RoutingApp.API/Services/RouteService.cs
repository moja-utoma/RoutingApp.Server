using RoutingApp.API.Data.Entities;
using RoutingApp.API.Mappers;
using RoutingApp.API.Models.DTO;
using RoutingApp.API.Models.Response.GetAll;
using RoutingApp.API.Repositories.Interfaces;
using RoutingApp.API.Services.Interfaces;
using Route = RoutingApp.API.Data.Entities.Route;

namespace RoutingApp.API.Services
{
	public class RouteService : IRouteService
	{
		private readonly IRepository<Route> _routeRepository;
		private readonly IPointRepository<DeliveryPoint> _deliveryPointRepository;
		private readonly IPointRepository<Warehouse> _warehouseRepository;

		public RouteService(IRepository<Route> routeRepository,
		IPointRepository<DeliveryPoint> deliveryPointRepository,
		IPointRepository<Warehouse> warehouseRepository)
		{
			_routeRepository = routeRepository;
			_deliveryPointRepository = deliveryPointRepository;
			_warehouseRepository = warehouseRepository;
		}

		public async Task<RouteResponseDTO> CreateRouteAsync(CreateRouteRequestDTO request)
		{
			var warehouses = await _warehouseRepository.GetMultipleByIdAsync(request.WarehouseIds);
			if (!warehouses.Any())
			{
				throw new Exception("At least one warehouse is required");
			}

			var points = await _deliveryPointRepository.GetMultipleByIdAsync(request.DeliveryPointIds);
			if (!warehouses.Any() || !points.Any())
			{
				throw new Exception("No valid delivery points found for given IDs");
			}

			var entity = ModelToEntity.CreateEntityFromRoute(request, points, warehouses);

			var result = await _routeRepository.AddAsync(entity);
			await _routeRepository.SaveChangesAsync();

			var dto = EntityToModel.CreateModelFromRoute(result);
			return dto;
		}

		public async Task<IEnumerable<RouteResponseDTO>> GetAllRoutesAsync()
		{
			var result = await _routeRepository.GetAllAsync();
			return EntityToModel.CreateModelsFromRoutes(result);
		}

		public async Task<RouteResponseDTO?> GetRouteByIDAsync(int id)
		{
			var result = await _routeRepository.GetByIdAsync(id);
			if (result == null)
			{
				throw new Exception("Not found");
			}

			return EntityToModel.CreateModelFromRoute(result);
		}

		public async Task DeleteAsync(int id)
		{
			var entity = await _routeRepository.GetByIdAsync(id);
			if (entity == null)
			{
				throw new Exception("No route with such ID found");
			}

			//deletes relationship with points
            //if (entity.DeliveryPoints != null && entity.DeliveryPoints.Any())
            //{
            //    entity.DeliveryPoints = new List<DeliveryPoint>();
            //}

            //if (entity.Warehouses != null && entity.Warehouses.Any())
            //{
            //    entity.Warehouses = new List<Warehouse>();
            //}

            _routeRepository.Delete(entity);
			await _routeRepository.SaveChangesAsync();
		}

		public async Task<RouteResponseDTO> EditAsync(EditRouteRequestDTO request)
		{
			var entity = await _routeRepository.GetByIdAsync(request.Id);
			if (entity == null)
			{
				throw new Exception("No route with such ID found");
			}

			entity.Name = request.Name;

			var warehouses = await _warehouseRepository.GetMultipleByIdAsync(request.WarehouseIds);
            if (warehouses.Count() != request.WarehouseIds.Count())
            {
				throw new Exception("Some warehouses are invalid");
            }

			var deliveryPoints = await _deliveryPointRepository.GetMultipleByIdAsync(request.DeliveryPointIds);
            if (deliveryPoints.Count() != request.DeliveryPointIds.Count())
            {
                throw new Exception("Some delivery points are invalid");
            }

			entity.Warehouses = warehouses;
			entity.DeliveryPoints = deliveryPoints;

            await _routeRepository.SaveChangesAsync();
			return EntityToModel.CreateModelFromRoute(entity);
		}
	}
}
