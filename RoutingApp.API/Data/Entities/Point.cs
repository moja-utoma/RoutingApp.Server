using System.Text.Json.Serialization;

namespace RoutingApp.API.Data.Entities
{
	public class Point : ISoftDeletable
	{
		public int Id { get; set; }
		public required string Name { get; set; }
		public required string Address { get; set; }
		public decimal Longitude { get; set; }
		public decimal Latitude { get; set; }

		public ICollection<Route>? Routes { get; set; }

        public bool IsDeleted { get; set; }
		public DateTimeOffset? DeletedAt { get; set; }
	}
}
