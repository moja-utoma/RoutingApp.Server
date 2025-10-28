namespace RoutingApp.API.Data.Entities
{
	public class Route :ISoftDeletable
	{
		public int Id { get; set; }
		public required string Name { get; set; }

		public required IEnumerable<Warehouse> Warehouses { get; set; }
		public required IEnumerable<DeliveryPoint> DeliveryPoints { get; set; }
		//public required IEnumerable<Vehicle> Vehicles { get; set; }
		public IEnumerable<CalculatedRoute>? CalculatedRoutes { get; set; }

        public string Status { get; set; } = "Pending"; // Pending, Processing, Completed, Failed
        public DateTime? UpdatedAt { get; set; }
        public string? CorrelationId { get; set; }

        public bool IsDeleted { get; set; }
		public DateTimeOffset? DeletedAt { get; set; }
	}
}
