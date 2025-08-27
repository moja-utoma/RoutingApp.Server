using RoutingApp.API.Models.Responses.Warehouse;

namespace RoutingApp.API.Models.Responses.Vehicles
{
    public class VehicleDetailsResponseDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Capacity { get; set; }
        public WarehouseResponseDTO? Warehouse{ get; set; }
    }
}
