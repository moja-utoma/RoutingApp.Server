using RoutingApp.API.Models.DTO;

namespace RoutingApp.API.Services.Interfaces
{
	public interface IRouteService : IDeletable
	{
		Task<IEnumerable<RouteResponse>> GetAllRoutesAsync();
		Task<RouteResponse?> GetRouteByIDAsync(int id);
		Task<RouteResponse> CreateRouteAsync(CreateRouteRequest request);
		Task<RouteResponse> EditAsync(EditRouteRequest request);
	}
}
