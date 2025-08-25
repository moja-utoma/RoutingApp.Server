namespace RoutingApp.API.Models.DTO
{
	public class RouteResponse
	{
		public int Id { get; set; }
		public required string Name { get; set; }

		public required IEnumerable<WarehouseResponse> Warehouses { get; set; } // -
		// w id
		// w name
		public required IEnumerable<DeliveryPointResponse> DeliveryPoints { get; set; } // -
		// dp id
		// dp quantity


		//public required IEnumerable<Vehicle> Vehicles { get; set; }
	}
}
