using RoutingApp.Data.Entities;
using RoutingApp.API.Models;
using RoutingApp.API.Models.DTO;
using RoutingApp.API.Models.Responses;
using RoutingApp.API.Models.Responses.Warehouse;
using RoutingApp.API.Models.Responses.Warehouses;
using RoutingApp.Data.Repositories;

namespace RoutingApp.API.Services.Interfaces
{
	public interface IWarehouseService : IDeletable
	{
		Task<PaginatedResponseDTO<WarehouseResponseDTO>> GetAllPointsAsync(QueryParametersModel filters);
		Task<WarehouseDetailsResponseDTO?> GetPointByIDAsync(int id);
		Task<List<string>> ImportCSV(IFormFile file);
		Task<WarehouseResponseDTO> CreatePointAsync(CreateWarehouseRequestDTO point);
		Task<WarehouseResponseDTO> EditAsync(EditWarehouseRequestDTO request);
	}
}
