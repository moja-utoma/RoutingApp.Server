using RoutingApp.API.Models.Responses.Base;

namespace RoutingApp.API.Models.Responses.DeliveryPoints
{
	public class DeliveryPointResponseDTO : PointResponseDTO
	{
		public decimal Weight { get; set; }
	}
}
