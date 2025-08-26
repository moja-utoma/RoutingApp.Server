using RoutingApp.API.Models.DTO;
using RoutingApp.API.Models.Response.GetAll;

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
