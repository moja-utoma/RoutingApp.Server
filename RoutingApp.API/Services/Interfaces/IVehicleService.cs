using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoutingApp.API.Models.Request;
using RoutingApp.API.Models.Responses.Vehicles;

namespace RoutingApp.API.Services.Interfaces
{
    public interface IVehicleService : IDeletable
    {
        Task<IEnumerable<VehicleResponseDTO>> GetAllVehiclesAsync();
        Task<VehicleResponseDTO?> GetVehicleByIDAsync(int id);
        Task<VehicleResponseDTO> CreateVehicleAsync(CreateVehicleRequestDTO request);
        Task<VehicleResponseDTO> EditAsync(EditVehicleRequestDTO request);
    }
}