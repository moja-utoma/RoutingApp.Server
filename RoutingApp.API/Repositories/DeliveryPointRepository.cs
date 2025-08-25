using Microsoft.EntityFrameworkCore;
using RoutingApp.API.Data;
using RoutingApp.API.Data.Entities;
using RoutingApp.API.Enumerations;
using RoutingApp.API.Models;
using RoutingApp.API.Repositories.Interfaces;
using Route = RoutingApp.API.Data.Entities.Route;

namespace RoutingApp.API.Repositories
{
    public class DeliveryPointRepository : Repository<DeliveryPoint>, IPointRepository<DeliveryPoint>
    {
        private readonly AppDbContext _context;
        //private readonly DbSet<Point> _dbSet;

        public DeliveryPointRepository(AppDbContext context) : base(context)
        {
            _context = context;
            //_dbSet = context.Set<Point>();
        }

        public async Task<IEnumerable<DeliveryPoint>> GetAllWithParamsAsync(FromQueryParametersModel filters)
        {
            IQueryable<DeliveryPoint> query = _context.Set<DeliveryPoint>();
            if (!string.IsNullOrWhiteSpace(filters.SearchString))
            {
                query = query.Where(e =>
                    EF.Functions.Like(e.Name, $"%{filters.SearchString}%") ||
                    EF.Functions.Like(e.Address, $"%{filters.SearchString}%"));
            }

            query = filters.IsDesc
                ? query.OrderByDescending(e => EF.Property<object>(e, filters.OrderBy))
                : query.OrderBy(e => EF.Property<object>(e, filters.OrderBy));

            query = query
                .Skip((filters.Page - 1) * filters.PageSize)
                .Take(filters.PageSize);

            return await query.ToListAsync();
        }
    }
}

