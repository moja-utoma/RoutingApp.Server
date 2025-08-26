namespace RoutingApp.API.Models.DTO
{
	public class CreateWarehouseRequestDTO : CreatePointRequestDTO
	{
		public IEnumerable<int>? VehicleIds { get; set; }
	}
}
