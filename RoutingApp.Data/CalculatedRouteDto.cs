namespace RoutingApp.Data
{
    public class CalculatedRouteDto
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public string Calculation { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
