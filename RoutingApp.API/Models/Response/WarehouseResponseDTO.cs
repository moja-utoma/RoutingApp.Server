using RoutingApp.API.Enumerations;

namespace RoutingApp.API.Models.DTO
{
	public class WarehouseResponseDTO : PointResponseDTO
	{
		public int VehicleQuantity { get; set; }
	}
}
