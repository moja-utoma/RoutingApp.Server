namespace RoutingApp.API.Models.DTO
{
	public class CreateWarehouseRequest : CreatePointRequest
	{
		public IEnumerable<int>? VehicleIds { get; set; }
	}
}
