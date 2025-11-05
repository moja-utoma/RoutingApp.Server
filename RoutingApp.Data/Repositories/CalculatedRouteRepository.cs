using Microsoft.EntityFrameworkCore;
using RoutingApp.Data.Entities;
using RoutingApp.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutingApp.Data.Repositories
{
	public interface ICalculatedRouteRepository : IRepository<CalculatedRoute>
	{
		Task<CalculatedRoute?> GetCalculatedRouteByRouteIdAsync(int routeId);
	}

	public class CalculatedRouteRepository: Repository<CalculatedRoute>, ICalculatedRouteRepository
	{
		private readonly AppDbContext _context;
		public CalculatedRouteRepository(AppDbContext context) : base(context)
		{
			_context = context;
		}
		public async Task<CalculatedRoute?> GetCalculatedRouteByRouteIdAsync (int routeId)
		{
			return await _context.Set<CalculatedRoute>()
			.Include(r => r.Route)
			.Where(r => r.Route.Id == routeId)
			.OrderByDescending(r => r.CreatedAt)
			.FirstOrDefaultAsync();
		}
	}
}
