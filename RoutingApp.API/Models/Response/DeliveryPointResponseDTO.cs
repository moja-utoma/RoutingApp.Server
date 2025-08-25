using RoutingApp.API.Enumerations;

namespace RoutingApp.API.Models.DTO
{
	public class DeliveryPointResponseDTO : PointResponseDTO
	{
		public decimal Weight { get; set; }
	}
}
