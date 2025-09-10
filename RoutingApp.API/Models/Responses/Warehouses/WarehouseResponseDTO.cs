using RoutingApp.API.Models.Responses.Base;

namespace RoutingApp.API.Models.Responses.Warehouse
{
	public class WarehouseResponseDTO : PointResponseDTO
	{
		public int VehicleQuantity { get; set; }
	}
}
