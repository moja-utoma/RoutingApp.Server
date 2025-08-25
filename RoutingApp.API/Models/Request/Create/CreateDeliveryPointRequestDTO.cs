namespace RoutingApp.API.Models.DTO
{
	public class CreateDeliveryPointRequestDTO : CreatePointRequestDTO
	{
		public decimal Weight { get; set; }
	}
}
