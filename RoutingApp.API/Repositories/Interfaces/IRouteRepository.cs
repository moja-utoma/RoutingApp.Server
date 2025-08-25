using RoutingApp.API.Data.Entities;
using Route = RoutingApp.API.Data.Entities.Route;

namespace RoutingApp.API.Repositories.Interfaces
{
	public interface IRouteRepository : IRepository<Route>
	{
		Task<IEnumerable<Route>> GetAllWithPointsAsync();
		Task<Route?> GetByIdWithPointsAsync(int id);
		//Task<IEnumerable<Route>> GetMultipleByIdWithPointsAsync(IEnumerable<int> ids);
	}
}
