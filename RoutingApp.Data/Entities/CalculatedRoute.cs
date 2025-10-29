namespace RoutingApp.Data.Entities
{
    public class CalculatedRoute : ISoftDeletable
    {
        public int Id { get; set; }
        public string Calculation { get; set; }
        public DateTime CreatedAt { get; set; }
        public Route Route { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
