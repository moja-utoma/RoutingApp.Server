using RoutingApp.API.Data.Entities;
using RoutingApp.API.Models.Responses.Routes;
using Route = RoutingApp.API.Data.Entities.Route;

namespace RoutingApp.API.Repositories.Interfaces
{
	public interface IRouteRepository : IRepository<Route>
	{
		Task<IEnumerable<Route>> GetAllWithPointsAsync();
		Task<Route?> GetByIdWithPointsAsync(int id);
		//Task<IEnumerable<Route>> GetMultipleByIdWithPointsAsync(IEnumerable<int> ids);
		Task<CalculatedRouteDto> SaveCalculatedRoute(int id, string route);
		Task<CalculatedRoute?> GetLatestCalculatedRoute(int routeId);

    }
}
