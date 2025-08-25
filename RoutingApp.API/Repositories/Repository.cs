using Microsoft.EntityFrameworkCore;
using RoutingApp.API.Data;
using RoutingApp.API.Enumerations;
using RoutingApp.API.Repositories.Interfaces;
using System.Linq.Expressions;

namespace RoutingApp.API.Repositories
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly AppDbContext _context;

		public Repository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<T>> GetAllAsync()
		{
			return await _context.Set<T>().ToListAsync();
		}

		public async Task<T?> GetByIdAsync(int id)
		{
			return await _context.Set<T>().FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
		}

		public async Task<IEnumerable<T>> GetMultipleByIdAsync(IEnumerable<int> ids)
		{
			return await _context.Set<T>().Where(e => ids.Contains(EF.Property<int>(e, "Id"))).ToListAsync();
		}

		public async Task<T> AddAsync(T entity)
		{
			await _context.Set<T>().AddAsync(entity);
			return entity;
		}

		public async Task AddRangeAsync(IEnumerable<T> entities)
		{
			await _context.Set<T>().AddRangeAsync(entities);
		}

		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}

		public void Delete(T entity)
		{
			_context.Set<T>().Remove(entity);
		}
	}
}
