namespace RoutingApp.API.Data.Entities
{
    public class Vehicle :ISoftDeletable
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Capacity { get; set; }

        public Warehouse? Warehouse { get; set; }
        //public IEnumerable<Route>? Routes { get; set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
