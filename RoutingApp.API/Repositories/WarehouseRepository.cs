using Microsoft.EntityFrameworkCore;
using RoutingApp.API.Data;
using RoutingApp.API.Data.Entities;
using RoutingApp.API.Models;
using RoutingApp.API.Repositories.Interfaces;
using Route = RoutingApp.API.Data.Entities.Route;

namespace RoutingApp.API.Repositories
{
    public class WarehouseRepository : Repository<Warehouse>, IPointRepository<Warehouse>
    {
        private readonly AppDbContext _context;
        //private readonly DbSet<Point> _dbSet;

        public WarehouseRepository(AppDbContext context) : base(context)
        {
            _context = context;
            //_dbSet = context.Set<Point>();
        }

        public IQueryable<Warehouse> GetAllWithParams(QueryParametersModel filters)
        {
            IQueryable<Warehouse> query = _context.Set<Warehouse>();
            if (!string.IsNullOrWhiteSpace(filters.SearchString))
            {
                query = query.Where(e =>
                    EF.Functions.Like(e.Name, $"%{filters.SearchString}%") ||
                    EF.Functions.Like(e.Address, $"%{filters.SearchString}%"));
            }

            var properties = typeof(Warehouse)
    .GetProperties()
    .Select(p => p.Name)
    .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var normalizedOrderBy = properties
                .FirstOrDefault(p => p.Equals(filters.OrderBy, StringComparison.OrdinalIgnoreCase))
                ?? "Name";
            query = filters.IsDesc
    ? query.OrderByDescending(e => EF.Property<object>(e, normalizedOrderBy))
    : query.OrderBy(e => EF.Property<object>(e, normalizedOrderBy));

            return query;
        }

    }
}

