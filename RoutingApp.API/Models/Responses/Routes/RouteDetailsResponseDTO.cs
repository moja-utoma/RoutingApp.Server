using RoutingApp.API.Models.Response.GetAll;
using RoutingApp.API.Models.Responses.DeliveryPoints;
using RoutingApp.API.Models.Responses.Warehouse;

namespace RoutingApp.API.Models.Responses.Routes
{
    public class RouteDetailsResponseDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required IEnumerable<WarehouseResponseDTO> Warehouses{ get; set; }
        public required IEnumerable<DeliveryPointResponseDTO> DeliveryPoints { get; set; }
    }
}
