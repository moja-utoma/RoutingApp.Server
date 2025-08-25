using RoutingApp.API.Enumerations;

namespace RoutingApp.API.Models.DTO
{
	public class DeliveryPointResponse : PointResponse
	{
		public decimal Weight { get; set; }
	}
}
