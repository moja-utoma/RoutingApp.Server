using RoutingApp.API.Models.Responses;

namespace RoutingApp.API.Services.Interfaces
{
    public interface IOrsService
    {
        Task<OrsSearchResponse?> SearchAddressAsync(string query);
        Task<string?> GetRouteAsync(string payloadJson);
        Task<OrsSearchResponse?> ReverseSearchAsync(double lat, double lng);
    }
}
