using RoutingApp.API.Models.Responses.Base;
using RoutingApp.API.Models.Responses.Vehicles;

namespace RoutingApp.API.Models.Responses.Warehouses
{
    public class WarehouseDetailsResponseDTO :PointResponseDTO
    {
        public IEnumerable<VehicleResponseDTO>? Vehicles { get; set; }
    }
}
