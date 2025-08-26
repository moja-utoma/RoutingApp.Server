using RoutingApp.API.Models.Response.GetAll;

namespace RoutingApp.API.Models.Response.GetByID
{
    public class DetailsWarehouseResponseDTO : DetailsPointResponseDTO
    {
        public IEnumerable<VehiclesForWarehouseDTO>? Vehicles { get; set; }
    }

    public class VehiclesForWarehouseDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Capacity { get; set; }
    }
}
