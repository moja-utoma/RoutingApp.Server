using RoutingApp.API.Data.Entities;
using RoutingApp.API.Enumerations;
using RoutingApp.API.Models;
using RoutingApp.API.Models.DTO;
using RoutingApp.API.Models.Responses.Warehouse;
using RoutingApp.API.Models.Responses.Warehouses;

namespace RoutingApp.API.Services.Interfaces
{
	public interface IWarehouseService : IDeletable
	{
		Task<IEnumerable<WarehouseResponseDTO>> GetAllPointsAsync(QueryParametersModel filters);
		Task<WarehouseDetailsResponseDTO?> GetPointByIDAsync(int id);
		Task<List<string>> ImportCSV(IFormFile file);
		Task<WarehouseResponseDTO> CreatePointAsync(CreateWarehouseRequestDTO point);
		Task<WarehouseResponseDTO> EditAsync(EditWarehouseRequestDTO request);
	}
}
