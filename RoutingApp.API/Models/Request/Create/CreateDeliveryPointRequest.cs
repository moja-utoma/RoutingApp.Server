namespace RoutingApp.API.Models.DTO
{
	public class CreateDeliveryPointRequest : CreatePointRequest
	{
		public decimal Weight { get; set; }
	}
}
