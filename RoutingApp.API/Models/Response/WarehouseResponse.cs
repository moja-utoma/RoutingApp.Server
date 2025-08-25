using RoutingApp.API.Enumerations;

namespace RoutingApp.API.Models.DTO
{
	public class WarehouseResponse : PointResponse
	{
		public int VehicleQuantity { get; set; }
	}
}
