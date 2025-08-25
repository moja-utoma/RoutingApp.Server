namespace RoutingApp.API.Models.DTO
{
	public class RouteResponseDTO
	{
		public int Id { get; set; }
		public required string Name { get; set; }

		public required IEnumerable<string> WarehouseNames { get; set; }
		public int DeliveryPointsQuantity { get; set; } 
	}
}
