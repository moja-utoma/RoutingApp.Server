using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RoutingApp.API.Data.Entities;
using RoutingApp.API.Extensions;
using RoutingApp.API.Mappers;
using RoutingApp.API.Models;
using RoutingApp.API.Models.DTO;
using RoutingApp.API.Models.Responses;
using RoutingApp.API.Models.Responses.DeliveryPoints;
using RoutingApp.API.Models.Responses.Warehouse;
using RoutingApp.API.Models.Responses.Warehouses;
using RoutingApp.API.Repositories.Interfaces;
using RoutingApp.API.Services.Interfaces;
using System.Globalization;
using System.Linq.Expressions;

namespace RoutingApp.API.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IPointRepository<Warehouse> _repository;
        private readonly IRepository<Vehicle> _vehicleRepository;

        public WarehouseService(IPointRepository<Warehouse> repository, IRepository<Vehicle> vehicleRepository)
        {
            _repository = repository;
            _vehicleRepository = vehicleRepository;
        }

        public async Task<PaginatedResponseDTO<WarehouseResponseDTO>> GetAllPointsAsync(QueryParametersModel filters)
        {
            var query = _repository.GetAll();
            query = query.ApplySearch(filters.SearchString);
            query = query.ApplySorting(SortMappings.GetValueOrDefault(filters.OrderBy, SortMappings["Name"]), filters.IsDesc);
            var paginated = await query.Select(ToDto).ApplyPagination(filters);
            //result.Items = result.Items.ApplySorting(filters.OrderBy, filters.IsDesc);

            return paginated;
         }

        public static readonly Dictionary<string, Expression<Func<Warehouse, object>>> SortMappings =
    new(StringComparer.OrdinalIgnoreCase)
    {
        ["Id"] = w => w.Id,
        ["Name"] = w => w.Name,
        ["Address"] = w => w.Address,
        ["VehicleQuantity"] = w => w.Vehicles.Count()
    };

        public static readonly Expression<Func<Warehouse, WarehouseResponseDTO>> ToDto =
        point => new WarehouseResponseDTO
        {
            Id = point.Id,
            Name = point.Name,
            Address = point.Address,
            Longitude = point.Longitude,
            Latitude = point.Latitude,
            VehicleQuantity = point.Vehicles != null ? point.Vehicles.Count() : 0,
        };

        public async Task<WarehouseDetailsResponseDTO?> GetPointByIDAsync(int id)
        {
            var result = await _repository.GetByIdAsync(id);
            if (result == null)
            {
                throw new Exception("Not found");
            }

            return EntityToModel.CreateModelForDetailsFromWarehouse(result);
        }

        public async Task<WarehouseResponseDTO> CreatePointAsync(CreateWarehouseRequestDTO point)
        {
            var entity = ModelToEntity.CreateEntityFromWarehouse(point);

            var result = await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            var dto = EntityToModel.CreateModelFromWarehouse(result);
            return dto;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new Exception("No point with such ID found");
            }

            if (entity.Routes != null && entity.Routes.Count > 0)
            {
                throw new Exception("Point is a part of a route and cannot be deleted");
            }

            _repository.Delete(entity);
            await _repository.SaveChangesAsync();
        }
        public async Task<WarehouseResponseDTO> EditAsync(EditWarehouseRequestDTO request)
        {
            var entity = await _repository.GetByIdAsync(request.Id);
            if (entity == null)
            {
                throw new Exception("No point with such ID found");
            }

            entity.Name = request.Name;
            entity.Address = request.Address;
            entity.Longitude = request.Longitude;
            entity.Latitude = request.Latitude;

            if (request.VehicleIds != null)
            {
                var vehicles = await _vehicleRepository.GetMultipleByIdAsync(request.VehicleIds);
                if (vehicles.Count() != request.VehicleIds.Count())
                {
                    throw new Exception("Some vehicles are invalid");
                }
                entity.Vehicles = vehicles.ToList();
            }

            await _repository.SaveChangesAsync();
            return EntityToModel.CreateModelFromWarehouse(entity);
        }

        public async Task<List<string>> ImportCSV(IFormFile file)
        {

            if (file == null || file.Length == 0)
            {
                throw new Exception("File is empty");
            }

            var allowedExtensions = new[] { ".csv" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                throw new Exception("Not acceptable file format, please upload a .csv file");
            }

            var points = new List<Warehouse>();
            var errors = new List<string>();
            int rowNumber = 1;

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                csv.Context.RegisterClassMap<WarehouseMap>();
                await foreach (var point in csv.GetRecordsAsync<Warehouse>())
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(point.Name) || string.IsNullOrWhiteSpace(point.Address))
                        {
                            errors.Add($"Row {rowNumber}: Missing Name or Address.");
                        }
                        else
                        {
                            points.Add(point);
                        }
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"Row {rowNumber}: {ex.Message}");
                    }

                    rowNumber++;
                }
            }

            if (points.Any())
            {
                await _repository.AddRangeAsync(points);
                await _repository.SaveChangesAsync();
            }

            return errors;
        }
    }
}
