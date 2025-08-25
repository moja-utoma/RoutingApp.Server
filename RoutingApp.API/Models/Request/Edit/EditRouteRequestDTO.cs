namespace RoutingApp.API.Models.DTO
{
	public class EditRouteRequestDTO
	{
		public int Id { get; set; }
		public required string Name { get; set; }

		public required IEnumerable<int> WarehouseIds { get; set; }
		public required IEnumerable<int> DeliveryPointIds { get; set; }
		//public required IEnumerable<int> VehiclesId { get; set; }
	}
}
