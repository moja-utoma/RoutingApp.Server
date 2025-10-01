using RoutingApp.API.Models.Responses;
using System.Runtime.CompilerServices;

namespace RoutingApp.API.Services.Interfaces
{
    public interface IOrsService
    {
        IAsyncEnumerable<double[]> StreamRouteAsync(
            string routeId,
            string payloadJson,
            CancellationToken cancellationToken);
        Task<OrsSearchResponse?> SearchAddressAsync(string query);
        Task<string?> GetRouteAsync(string payloadJson);
        Task<OrsSearchResponse?> ReverseSearchAsync(double lat, double lng);
    }
}
