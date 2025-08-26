namespace RoutingApp.API.Models.DTO
{
	public class EditWarehouseRequestDTO : EditPointRequestDTO
	{
		public IEnumerable<int>? VehicleIds { get; set; }
	}
}
