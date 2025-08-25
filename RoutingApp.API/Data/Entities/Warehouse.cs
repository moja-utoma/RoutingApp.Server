using RoutingApp.API.Data.Entities;

namespace RoutingApp.API.Data.Entities
{
	public class Warehouse : Point
	{
		public IEnumerable<Vehicle>? Vehicles { get; set; }
	}
}
