using RoutingApp.Data.Entities;
using RoutingApp.Data.Repositories.Interfaces;
using Route = RoutingApp.Data.Entities.Route;

namespace RoutingApp.Data.Repositories.Interfaces
{
    public interface IPointRepository<T> : IRepository<T> where T : Point
    {
        IQueryable<T> GetAllWithParams(QueryParametersModel filters);
    }
}
