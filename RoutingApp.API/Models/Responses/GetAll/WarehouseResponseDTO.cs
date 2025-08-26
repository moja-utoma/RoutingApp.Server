using RoutingApp.API.Enumerations;

namespace RoutingApp.API.Models.Response.GetAll
{
	public class WarehouseResponseDTO : PointResponseDTO
	{
		public int VehicleQuantity { get; set; }
	}
}
