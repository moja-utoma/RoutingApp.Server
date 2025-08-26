namespace RoutingApp.API.Models.DTO
{
	public class CreateRouteRequestDTO
	{
		public required string Name { get; set; }

		public required IEnumerable<int> WarehouseIds { get; set; }
		public required IEnumerable<int> DeliveryPointIds { get; set; }
		//public required IEnumerable<int> VehiclesId { get; set; }
	}
}
