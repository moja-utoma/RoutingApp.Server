using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoutingApp.API.Data.Entities;
using RoutingApp.API.Mappers;
using RoutingApp.API.Models.Request;
using RoutingApp.API.Models.Response;
using RoutingApp.API.Repositories.Interfaces;
using RoutingApp.API.Services.Interfaces;

namespace RoutingApp.API.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IRepository<Vehicle> _repository;
        private readonly IPointRepository<Warehouse> _warehouseRepository;

        public VehicleService(IRepository<Vehicle> repository, IPointRepository<Warehouse> warehouseRepository)
        {
            _repository = repository;
            _warehouseRepository = warehouseRepository;
        }

        public async Task<VehicleResponseDTO> CreateVehicleAsync(CreateVehicleRequestDTO request)
        {
            Warehouse? warehouse = null;
            if (request.WarehouseId != null)
            {
                warehouse = await _warehouseRepository.GetByIdAsync((int)request.WarehouseId);
            }

            var entity = ModelToEntity.CreateEntityFromVehicle(request, warehouse);
            var result = await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            var dto = EntityToModel.CreateModelFromVehicle(result);
            return dto;
        }

        public async Task<IEnumerable<VehicleResponseDTO>> GetAllVehiclesAsync()
        {
            var result = await _repository.GetAllAsync();
            return EntityToModel.CreateModelsFromVehicles(result);
        }

        public async Task<VehicleResponseDTO?> GetVehicleByIDAsync(int id)
        {
            var result = await _repository.GetByIdAsync(id);
            if (result == null)
            {
                throw new Exception("Not found");
            }

            return EntityToModel.CreateModelFromVehicle(result);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new Exception("No vehicle with such ID found");
            }

            //if (entity.Warehouse != null && entity.Warehouse.Vehicles.Count() ==1)
            //{
                
            //}

            _repository.Delete(entity);
            await _repository.SaveChangesAsync();
        }
        public async Task<VehicleResponseDTO> EditAsync(EditVehicleRequestDTO request)
        {
            var entity = await _repository.GetByIdAsync(request.Id);
            if (entity == null)
            {
                throw new Exception("No vehicle with such ID found");
            }

            entity.Name = request.Name;
            entity.Capacity = request.Capacity;

            if (request.WarehouseId != null)
            {
                var warehouse = await _warehouseRepository.GetByIdAsync((int)request.WarehouseId);
                if (warehouse == null)
                {
                    throw new Exception("No warehouse with such ID found");
                }
                entity.Warehouse = warehouse;
            }

            await _repository.SaveChangesAsync();
            return EntityToModel.CreateModelFromVehicle(entity);
        }
    }
}