using RoutingApp.API.Data.Entities;
using RoutingApp.API.Enumerations;
using RoutingApp.API.Models;
using Route = RoutingApp.API.Data.Entities.Route;

namespace RoutingApp.API.Repositories.Interfaces
{
    public interface IPointRepository<T> : IRepository<T> where T : Point
    {
        Task<IEnumerable<T>> GetAllWithParamsAsync(FromQueryParametersModel filters);
    }
}
