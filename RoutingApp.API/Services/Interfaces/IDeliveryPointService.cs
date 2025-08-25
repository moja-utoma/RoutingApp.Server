using RoutingApp.API.Data.Entities;
using RoutingApp.API.Enumerations;
using RoutingApp.API.Models;
using RoutingApp.API.Models.DTO;

namespace RoutingApp.API.Services.Interfaces
{
	public interface IDeliveryPointService : IDeletable
	{
		Task<IEnumerable<DeliveryPointResponse>> GetAllPointsAsync(FromQueryParametersModel filters);
		Task<DeliveryPointResponse?> GetPointByIDAsync(int id);
		Task<List<string>> ImportCSV(IFormFile file);
		Task<DeliveryPointResponse> CreatePointAsync(CreateDeliveryPointRequest point);
		Task<DeliveryPointResponse> EditAsync(EditDeliveryPointRequest request);
	}
}
