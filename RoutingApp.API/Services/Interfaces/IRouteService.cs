using RoutingApp.API.Models.DTO;

namespace RoutingApp.API.Services.Interfaces
{
	public interface IRouteService : IDeletable
	{
		Task<IEnumerable<RouteResponseDTO>> GetAllRoutesAsync();
		Task<RouteResponseDTO?> GetRouteByIDAsync(int id);
		Task<RouteResponseDTO> CreateRouteAsync(CreateRouteRequestDTO request);
		Task<RouteResponseDTO> EditAsync(EditRouteRequestDTO request);
	}
}
