using Microsoft.EntityFrameworkCore;
using RoutingApp.API.Data;
using RoutingApp.API.Data.Entities;
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

        public IQueryable<DeliveryPoint> GetAllWithParams(QueryParametersModel filters)
        {
            IQueryable<DeliveryPoint> query = _context.Set<DeliveryPoint>();
            if (!string.IsNullOrWhiteSpace(filters.SearchString))
            {
                query = query.Where(e =>
                    EF.Functions.Like(e.Name, $"%{filters.SearchString}%") ||
                    EF.Functions.Like(e.Address, $"%{filters.SearchString}%"));
            }

            var deliveryPointProperties = typeof(DeliveryPoint)
                .GetProperties()
                .Select(p => p.Name)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var normalizedOrderBy = deliveryPointProperties
                .FirstOrDefault(p => p.Equals(filters.OrderBy, StringComparison.OrdinalIgnoreCase))
                ?? "Name";
            query = filters.IsDesc
    ? query.OrderByDescending(e => EF.Property<object>(e, normalizedOrderBy))
    : query.OrderBy(e => EF.Property<object>(e, normalizedOrderBy));


            return query;
        }
    }
}

