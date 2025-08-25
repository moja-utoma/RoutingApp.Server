using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoutingApp.API.Models.Request;
using RoutingApp.API.Models.Response;

namespace RoutingApp.API.Services.Interfaces
{
    public interface IVehicleService : IDeletable
    {
        Task<IEnumerable<VehicleResponse>> GetAllVehiclesAsync();
        Task<VehicleResponse?> GetVehicleByIDAsync(int id);
        Task<VehicleResponse> CreateVehicleAsync(CreateVehicleRequest request);
        Task<VehicleResponse> EditAsync(EditVehicleRequest request);
    }
}