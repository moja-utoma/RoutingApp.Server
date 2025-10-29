using RoutingApp.Data.Entities;

namespace RoutingApp.Data.Entities
{
	public class DeliveryPoint : Point
	{
		public decimal Weight { get; set; }

		//public int? WarehouseID { get; set; }
		//public Warehouse? Warehouse { get; set; }
	}
}
