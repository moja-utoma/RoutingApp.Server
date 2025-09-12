using Microsoft.EntityFrameworkCore;
using RoutingApp.API.Data;
using RoutingApp.API.Data.Entities;
using RoutingApp.API.Models.Responses.Routes;
using RoutingApp.API.Repositories.Interfaces;
using Route = RoutingApp.API.Data.Entities.Route;

namespace RoutingApp.API.Repositories
{
    public class RouteRepository : Repository<Route>, IRouteRepository
    {
        private readonly AppDbContext _context;
        //private readonly DbSet<Route> _dbSet;

        public RouteRepository(AppDbContext context) : base(context)
        {
            _context = context;
            //_dbSet = context.Set<Route>();
        }

        public async Task<IEnumerable<Route>> GetAllWithPointsAsync()
        {
            return await _context.Set<Route>()
            .Include(r => r.Warehouses)
            .Include(r => r.DeliveryPoints)
            .ToListAsync();
        }

        public async Task<Route?> GetByIdWithPointsAsync(int id)
        {
            return await _context.Set<Route>()
            .Include(r => r.Warehouses)
            .Include(r => r.DeliveryPoints)
            .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<CalculatedRouteDto> SaveCalculatedRoute(int id, string routeJson)
        {
            var routeEntity = await _context.Routes.FindAsync(id);
            if (routeEntity == null)
            {
                throw new InvalidOperationException($"Route with ID {id} not found.");
            }

            var calculatedRoute = new CalculatedRoute
            {
                Calculation = routeJson,
                Route = routeEntity,
                CreatedAt = DateTime.UtcNow
            };

            _context.CalculatedRoutes.Add(calculatedRoute);

            return new CalculatedRouteDto
            {
                Id = calculatedRoute.Id,
                RouteId = routeEntity.Id,
                Calculation = routeJson,
                CreatedAt = calculatedRoute.CreatedAt
            };
        }

        // public async Task<IEnumerable<Route>> GetMultipleByIdWithPointsAsync(IEnumerable<int> ids)
        // {
        // 	return await _dbSet.Include(r => r.Points)
        // 		.Where(r => ids.Contains(r.Id))
        // 		.ToListAsync();
        // }
    }
}
