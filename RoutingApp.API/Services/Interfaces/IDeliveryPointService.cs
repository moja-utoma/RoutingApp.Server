using RoutingApp.API.Data.Entities;
using RoutingApp.API.Enumerations;
using RoutingApp.API.Models;
using RoutingApp.API.Models.DTO;
using RoutingApp.API.Models.Responses.DeliveryPoints;

namespace RoutingApp.API.Services.Interfaces
{
	public interface IDeliveryPointService : IDeletable
	{
		Task<IEnumerable<DeliveryPointResponseDTO>> GetAllPointsAsync(QueryParametersModel filters);
		Task<DeliveryPointResponseDTO?> GetPointByIDAsync(int id);
		Task<List<string>> ImportCSV(IFormFile file);
		Task<DeliveryPointResponseDTO> CreatePointAsync(CreateDeliveryPointRequestDTO point);
		Task<DeliveryPointResponseDTO> EditAsync(EditDeliveryPointRequestDTO request);
	}
}
