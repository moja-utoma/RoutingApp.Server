using Microsoft.EntityFrameworkCore;
using RoutingApp.API.Data.Entities;
using RoutingApp.API.Models;
using RoutingApp.API.Models.Responses;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace RoutingApp.API.Extensions
{
    public static class IQueryableExtension
    {
        public static async Task<PaginatedResponseDTO<T>> ApplyPagination<T>(this IQueryable<T> query,
            QueryParametersModel filters) where T : class
        {
            //if (!string.IsNullOrWhiteSpace(filters.SearchString))
            //{
            //    var stringProps = typeof(T)
            //        .GetProperties()
            //        .Where(p => p.PropertyType == typeof(string)) //|| p.PropertyType == typeof(decimal)
            //        .Select(p => p.Name)
            //        .ToArray();

            //    query = query.ApplySearch(filters.SearchString, stringProps);
            //}

            //query = query.ApplySorting(filters.OrderBy, filters.IsDesc);

            //var propertiesRaw = typeof(T).GetProperties();
            //var propsFiltered = propertiesRaw
            //.Where(p => p.PropertyType == typeof(string) || p.PropertyType == typeof(decimal) || p.PropertyType == typeof(int));

            //var properties = propsFiltered.Select(p => p.Name)
            //    .ToHashSet(StringComparer.OrdinalIgnoreCase);

            //var normalizedOrderBy = properties
            //    .FirstOrDefault(p => p.Equals(filters.OrderBy, StringComparison.OrdinalIgnoreCase))
            //    ?? properties.FirstOrDefault() ?? "Id";

            //query = filters.IsDesc
            //    ? query.OrderByDescending(e => EF.Property<object>(e, normalizedOrderBy))
            //    : query.OrderBy(e => EF.Property<object>(e, normalizedOrderBy));

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((filters.Page - 1) * filters.PageSize)
                .Take(filters.PageSize)
                .ToListAsync();

            return new PaginatedResponseDTO<T>
            {
                Items = items,
                TotalCount = totalCount
            };
        }

        public static IQueryable<T> ApplySorting<T>(
            this IQueryable<T> query,
            string orderBy,
            bool isDesc) where T : class
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                return query;

            var property = typeof(T).GetProperty(orderBy,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (property == null)
                return query;

            var param = Expression.Parameter(typeof(T), "e");
            Expression sortExpression;

            // Handle collections by sorting on Count()
            if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && property.PropertyType != typeof(string))
            {
                var propertyAccess = Expression.Property(param, property);

                var countProperty = typeof(Enumerable)
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .FirstOrDefault(m => m.Name == "Count" && m.GetParameters().Length == 1)?
                    .MakeGenericMethod(property.PropertyType.GetGenericArguments().FirstOrDefault() ?? typeof(object));

                if (countProperty == null)
                    return query;

                sortExpression = Expression.Call(countProperty, propertyAccess);
            }
            // Handle simple types directly
            else if (property.PropertyType.IsPrimitive || property.PropertyType == typeof(string) || property.PropertyType == typeof(decimal) || property.PropertyType == typeof(DateTime))
            {
                sortExpression = Expression.Convert(Expression.Property(param, property), typeof(object));
            }
            else
            {
                // Unsupported complex type
                return query;
            }

            var lambda = Expression.Lambda<Func<T, object>>(sortExpression, param);

            return isDesc
                ? query.OrderByDescending(lambda)
                : query.OrderBy(lambda);
        }

        public static IQueryable<T> ApplySorting<T>(
        this IQueryable<T> query,
        Expression<Func<T, object>> sortExpression,
        bool isDescending)
        {
            if (sortExpression == null)
                return query;

            return isDescending
                ? query.OrderByDescending(sortExpression)
                : query.OrderBy(sortExpression);
        }

        public static IQueryable<T> ApplySearch<T>(
            this IQueryable<T> query,
            string searchString) where T : class
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return query;

            var searchableProperties = typeof(T)
                    .GetProperties()
                    .Where(p => p.PropertyType == typeof(string))
                    .Select(p => p.Name)
                    .ToArray();

            var parameter = Expression.Parameter(typeof(T), "e");
            Expression predicate = null;

            foreach (var propName in searchableProperties)
            {
                var property = typeof(T).GetProperty(propName,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (property != null && (property.PropertyType == typeof(string))) // || property.PropertyType == typeof(decimal)
                {
                    var propertyAccess = Expression.Property(parameter, property);

                    // Get the static method info for EF.Functions.Like
                    var likeMethod = typeof(DbFunctionsExtensions)
                        .GetMethod(nameof(DbFunctionsExtensions.Like), new[] {
                            typeof(DbFunctions),
                            typeof(string),
                            typeof(string)
                        });

                    // Create EF.Functions as a constant expression
                    var efFunctions = Expression.Constant(EF.Functions);

                    // Create the search pattern as a constant expression
                    var pattern = Expression.Constant($"%{searchString}%");

                    // Build the method call: EF.Functions.Like(e.PropertyName, "%searchString%")
                    var likeCall = Expression.Call(likeMethod, efFunctions, propertyAccess, pattern);


                    predicate = predicate == null
                        ? likeCall
                        : Expression.OrElse(predicate, likeCall);
                }
            }

            if (predicate == null)
                return query;

            var lambda = Expression.Lambda<Func<T, bool>>(predicate, parameter);
            return query.Where(lambda);
        }
    }
}
