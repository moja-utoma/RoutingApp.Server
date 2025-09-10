using RoutingApp.API.Data.Entities;

namespace RoutingApp.API.Data.Entities
{
	public class Warehouse : Point
	{
		public ICollection<Vehicle>? Vehicles { get; set; }
	}
}
