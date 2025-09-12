namespace RoutingApp.API.Models.ThirdParty
{
    public class RouteCalculationResponse
    {
        public int Id { get; set; }
        public List<Cluster> Clusters { get; set; }
        public double TotalCost { get; set; }
    }
    public class RoutePoint
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Order { get; set; }
    }

    public class Cluster
    {
        public double Cost { get; set; }
        public List<List<RoutePoint>> Routes { get; set; }
        public int WarehouseId { get; set; }
    }

}
