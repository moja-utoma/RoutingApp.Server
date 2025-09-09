using Microsoft.EntityFrameworkCore;

namespace RoutingApp.API.Models.Responses
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

        public static async Task<PaginatedList<TDestination>> CreateAsync<TSource, TDestination>(
       IQueryable<TSource> source,
       int pageIndex,
       int pageSize,
       Func<TSource, TDestination> mapFunc)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            var mappedItems = items.Select(mapFunc).ToList();
            return new PaginatedList<TDestination>(mappedItems, count, pageIndex, pageSize);
        }
    }
}
