namespace RoutingApp.API.Data.Entities
{
    public class CalculatedRoute
    {
        public int Id { get; set; }
        public string Calculation { get; set; }
        public DateTime CreatedAt { get; set; }
        public Route Route { get; set; }
    }
}
