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
using RoutingApp.API.Repositories.Interfaces;
using RoutingApp.API.Services.Interfaces;
using System.Globalization;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RoutingApp.API.Services
{
    public class DeliveryPointService : IDeliveryPointService
    {
        private readonly IPointRepository<DeliveryPoint> _repository;

        public DeliveryPointService(IPointRepository<DeliveryPoint> repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedResponseDTO<DeliveryPointResponseDTO>> GetAllPointsAsync(QueryParametersModel filters)
        {
            var query = _repository.GetAll();
            query = query.ApplySearch(filters.SearchString);
            query = query.ApplySorting(SortMappings.GetValueOrDefault(filters.OrderBy, SortMappings["Name"]), filters.IsDesc);
            var result = await query.Select(ToDto)
                .ApplyPagination(filters);
            //_repository.GetAllWithParams(filters);

            //result.Items = result.Items.ApplySorting(filters.OrderBy, filters.IsDesc);

            return result;
        }


        public static readonly Dictionary<string, Expression<Func<DeliveryPoint, object>>> SortMappings =
    new(StringComparer.OrdinalIgnoreCase)
    {
        ["Id"] = w => w.Id,
        ["Name"] = w => w.Name,
        ["Address"] = w => w.Address,
        ["Weight"] = w => w.Weight,

    };

        public static readonly Expression<Func<DeliveryPoint, DeliveryPointResponseDTO>> ToDto =
        point => new DeliveryPointResponseDTO
        {
            Id = point.Id,
            Name = point.Name,
            Address = point.Address,
            Longitude = point.Longitude,
            Latitude = point.Latitude,
            Weight = point.Weight
        };

        public async Task<DeliveryPointDetailsResponseDTO?> GetPointByIDAsync(int id)
        {
            var result = await _repository.GetByIdAsync(id);
            if (result == null)
            {
                throw new Exception("Not found");
            }

            return EntityToModel.CreateModelForDetailsFromDeliveryPoint(result);
        }

        public async Task<DeliveryPointResponseDTO> CreatePointAsync(CreateDeliveryPointRequestDTO point)
        {
            if (point.Weight < 0.1m)
            {
                throw new Exception("Weight cannot be less than 0.1 kg for delivery point");
            }

            var entity = ModelToEntity.CreateEntityFromDeliveryPoint(point);

            var result = await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            var dto = EntityToModel.CreateModelFromDeliveryPoint(result);
            return dto;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new Exception("No point with such ID found");
            }

            if (!entity.Routes.IsNullOrEmpty())
            {
                throw new Exception("Point is a part of a route and cannot be deleted");
            }

            _repository.Delete(entity);
            await _repository.SaveChangesAsync();
        }

        public async Task<DeliveryPointResponseDTO> EditAsync(EditDeliveryPointRequestDTO request)
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
            entity.Weight = request.Weight;


            await _repository.SaveChangesAsync();
            return EntityToModel.CreateModelFromDeliveryPoint(entity);
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

            var points = new List<DeliveryPoint>();
            var errors = new List<string>();
            int rowNumber = 1;

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                csv.Context.RegisterClassMap<DeliveryPointMap>();

                await foreach (var point in csv.GetRecordsAsync<DeliveryPoint>())
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(point.Name) || string.IsNullOrWhiteSpace(point.Address))
                        {
                            errors.Add($"Row {rowNumber}: Missing Name or Address.");
                        }
                        else if (point.Weight < 0.1m || point.Weight > 9999.99m)
                        {
                            errors.Add($"Row {rowNumber}: Invalid weight '{point.Weight}' for delivery point '{point.Name}'. Must be between 0.10 and 9999.99.");
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
