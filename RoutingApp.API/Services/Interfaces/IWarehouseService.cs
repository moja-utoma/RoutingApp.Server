using RoutingApp.API.Data.Entities;
using RoutingApp.API.Enumerations;
using RoutingApp.API.Models;
using RoutingApp.API.Models.DTO;

namespace RoutingApp.API.Services.Interfaces
{
	public interface IWarehouseService : IDeletable
	{
		Task<IEnumerable<WarehouseResponse>> GetAllPointsAsync(FromQueryParametersModel filters);
		Task<WarehouseResponse?> GetPointByIDAsync(int id);
		Task<List<string>> ImportCSV(IFormFile file);
		Task<WarehouseResponse> CreatePointAsync(CreateWarehouseRequest point);
		Task<WarehouseResponse> EditAsync(EditWarehouseRequest request);
	}
}
