using RoutingApp.Data.Entities;

namespace RoutingApp.Data.Entities
{
	public class Warehouse : Point
	{
		public ICollection<Vehicle>? Vehicles { get; set; }
	}
}
