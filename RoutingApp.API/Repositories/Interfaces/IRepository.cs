using System.Linq.Expressions;
using RoutingApp.API.Enumerations;

namespace RoutingApp.API.Repositories.Interfaces
{
	public interface IRepository<T> where T : class
	{
		Task<IEnumerable<T>> GetAllAsync();
		Task<T?> GetByIdAsync(int id);
		Task<IEnumerable<T>> GetMultipleByIdAsync(IEnumerable<int> ids);
		Task<T> AddAsync(T entity);
		Task AddRangeAsync(IEnumerable<T> entities);
		void Delete(T entity);
		Task SaveChangesAsync();
		//Task<IEnumerable<T>> GetAllWithParamsAsync(SortOrder order, string searchString, int page, int pageSize);
	}
}
