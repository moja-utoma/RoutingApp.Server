using RoutingApp.API.Enumerations;

namespace RoutingApp.API.Models.Response.GetAll
{
	public class DeliveryPointResponseDTO : PointResponseDTO
	{
		public decimal Weight { get; set; }
	}
}
