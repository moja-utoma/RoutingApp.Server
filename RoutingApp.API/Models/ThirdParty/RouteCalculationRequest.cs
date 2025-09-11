namespace RoutingApp.API.Models.ThirdParty
{
    public class RouteCalculationRequest
    {
        public int Id { get; set; }
        public List<WarehouseForCalculationDTO> Warehouses { get; set; }
        public List<VehicleForCalculationDTO> Vehicles { get; set; }
        public List<DeliveryPointForCalculationDTO> Points { get; set; }
    }

    public class WarehouseForCalculationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }

    public class VehicleForCalculationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Capacity { get; set; }
        public int Warehouse { get; set; }
    }

    public class DeliveryPointForCalculationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Weight { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
