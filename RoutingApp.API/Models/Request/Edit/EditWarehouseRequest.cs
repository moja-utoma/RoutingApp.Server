namespace RoutingApp.API.Models.DTO
{
	public class EditWarehouseRequest : EditPointRequest
	{
		public IEnumerable<int>? VehicleIds { get; set; }
	}
}
