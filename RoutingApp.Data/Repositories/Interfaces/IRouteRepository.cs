
using RoutingApp.Data.Entities;
using RoutingApp.Data.Repositories.Interfaces;
using Route = RoutingApp.Data.Entities.Route;

namespace RoutingApp.Data.Repositories.Interfaces
{
	public interface IRouteRepository : IRepository<Route>
	{
		Task<IEnumerable<Route>> GetAllWithPointsAsync();
		Task<Route?> GetByIdWithPointsAsync(int id);
		//Task<IEnumerable<Route>> GetMultipleByIdWithPointsAsync(IEnumerable<int> ids);
		Task<CalculatedRouteDto> SaveCalculatedRoute(int id, string route);
		Task<CalculatedRoute?> GetLatestCalculatedRoute(int routeId);
		Task<Route?> GetByCorrelationIdAsync(string id);

	}
}
