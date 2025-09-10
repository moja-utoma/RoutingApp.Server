using RoutingApp.API.Data.Entities;
using RoutingApp.API.Models;
using RoutingApp.API.Models.DTO;
using RoutingApp.API.Models.Responses;
using RoutingApp.API.Models.Responses.DeliveryPoints;

namespace RoutingApp.API.Services.Interfaces
{
	public interface IDeliveryPointService : IDeletable
	{
		Task<PaginatedResponseDTO<DeliveryPointResponseDTO>> GetAllPointsAsync(QueryParametersModel filters);
		Task<DeliveryPointDetailsResponseDTO?> GetPointByIDAsync(int id);
		Task<List<string>> ImportCSV(IFormFile file);
		Task<DeliveryPointResponseDTO> CreatePointAsync(CreateDeliveryPointRequestDTO point);
		Task<DeliveryPointResponseDTO> EditAsync(EditDeliveryPointRequestDTO request);
	}
}
