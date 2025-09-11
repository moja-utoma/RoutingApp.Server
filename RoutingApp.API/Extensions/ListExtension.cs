using RoutingApp.API.Models.Responses;
using System.Reflection;

namespace RoutingApp.API.Extensions
{
    public static class ListExtension
    {
        public static List<T> ApplySorting<T>(
            this List<T> response,
            string orderBy,
            bool isDesc)
        {
            if (response == null || response.Count == 0)
                return response;

            // Step 1: Filter sortable properties
            var propertiesRaw = typeof(T).GetProperties();
            var propsFiltered = propertiesRaw
                .Where(p => p.PropertyType == typeof(string)
                         || p.PropertyType == typeof(decimal)
                         || p.PropertyType == typeof(int));

            var properties = propsFiltered
                .Select(p => p.Name)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            // Step 2: Normalize orderBy
            var normalizedOrderBy = properties
                .FirstOrDefault(p => p.Equals(orderBy, StringComparison.OrdinalIgnoreCase))
                ?? properties.FirstOrDefault() ?? "Id";

            // Step 3: Get property info
            var propInfo = typeof(T).GetProperty(normalizedOrderBy,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propInfo == null)
                return response;

            // Step 4: Apply sorting
            Func<T, object> keySelector = x => propInfo.GetValue(x, null);

            response = isDesc
                ? response.OrderByDescending(keySelector).ToList()
                : response.OrderBy(keySelector).ToList();

            return response;
        }
    }
}
