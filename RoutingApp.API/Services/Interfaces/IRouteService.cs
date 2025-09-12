using RoutingApp.API.Models.DTO;
using RoutingApp.API.Models.Responses.Routes;
using RoutingApp.API.Models.ThirdParty;

namespace RoutingApp.API.Services.Interfaces
{
	public interface IRouteService : IDeletable
	{
		Task<IEnumerable<RouteResponseDTO>> GetAllRoutesAsync();
		Task<RouteDetailsResponseDTO?> GetRouteByIDAsync(int id);
		Task<RouteResponseDTO> CreateRouteAsync(CreateRouteRequestDTO request);
		Task<RouteResponseDTO> EditAsync(EditRouteRequestDTO request);
		Task<CalculatedRouteDto> CalculateRouteAsync(int id);

    }
}
