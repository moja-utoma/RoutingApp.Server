using Azure.Core;
using Microsoft.EntityFrameworkCore;
using RoutingApp.API.Data.Entities;
using RoutingApp.API.Mappers;
using RoutingApp.API.Models.DTO;
using RoutingApp.API.Models.Responses.Routes;
using RoutingApp.API.Models.ThirdParty;
using RoutingApp.API.Repositories.Interfaces;
using RoutingApp.API.Services.Interfaces;
using System.Net.Http;
using System.Text.Json;
using Route = RoutingApp.API.Data.Entities.Route;

namespace RoutingApp.API.Services
{
    public class RouteService : IRouteService
    {
        private readonly IRouteRepository _routeRepository;
        private readonly IPointRepository<DeliveryPoint> _deliveryPointRepository;
        private readonly IPointRepository<Warehouse> _warehouseRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public RouteService(IRouteRepository routeRepository,
        IPointRepository<DeliveryPoint> deliveryPointRepository,
        IPointRepository<Warehouse> warehouseRepository, IHttpClientFactory httpClientFactory)
        {
            _routeRepository = routeRepository;
            _deliveryPointRepository = deliveryPointRepository;
            _warehouseRepository = warehouseRepository;
            _httpClientFactory = httpClientFactory;
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
            var result = await _routeRepository.GetAll().ToListAsync();
            return EntityToModel.CreateModelsFromRoutes(result);
        }

        public async Task<RouteDetailsResponseDTO?> GetRouteByIDAsync(int id)
        {
            var result = await _routeRepository.GetByIdAsync(id);
            if (result == null)
            {
                throw new Exception("Not found");
            }

            return EntityToModel.CreateModelForDetailsFromRoute(result);
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

        public async Task<CalculatedRouteDto> CalculateRouteAsync(int id)
        {
            var route = await _routeRepository.GetByIdAsync(id);
            if (route == null)
                throw new Exception("Not found");

            var vehicles = route.Warehouses
                .Where(w => w.Vehicles != null)
                .SelectMany(w => w.Vehicles)
                .Select(v => new VehicleForCalculationDTO
                {
                    Id = v.Id,
                    Name = v.Name,
                    Capacity = v.Capacity,
                    Warehouse = v.Warehouse.Id
                })
                .ToList();


            var exportDto = new RouteCalculationRequest
            {
                Id = route.Id,
                Warehouses = route.Warehouses.Select(w => new WarehouseForCalculationDTO
                {
                    Id = w.Id,
                    Name = w.Name,
                    Latitude = w.Latitude,
                    Longitude = w.Longitude
                }).ToList(),
                Vehicles = vehicles,
                Points = route.DeliveryPoints.Select(p => new DeliveryPointForCalculationDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Weight = p.Weight,
                    Latitude = p.Latitude,
                    Longitude = p.Longitude
                }).ToList()
            };

            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.PostAsJsonAsync("http://127.0.0.1:5000/api/build-route", exportDto);
            //var jspnstring = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                // Save `id` and `jsonString` to DB
                var result = await _routeRepository.SaveCalculatedRoute(id, jsonString);
                await _routeRepository.SaveChangesAsync();
                return result;
            }
            else
            {
                // Handle error
                throw new Exception("error");
            }

            //if (response.IsSuccessStatusCode)
            //{
            //	var jsonString = await response.Content.ReadAsStringAsync();
            //	var result = JsonSerializer.Deserialize<RouteCalculationResponse>(jsonString, new JsonSerializerOptions
            //	{
            //		PropertyNameCaseInsensitive = true
            //	});
            //}

        }
    }
}
