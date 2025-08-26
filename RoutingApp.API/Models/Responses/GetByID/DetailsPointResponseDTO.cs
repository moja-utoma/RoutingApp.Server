using RoutingApp.API.Enumerations;

namespace RoutingApp.API.Models.Response.GetAll
{
	public class DetailsPointResponseDTO
	{
		public int Id { get; set; }
		public required string Name { get; set; }
		public required string Address { get; set; }
		public decimal Longitude { get; set; }
		public decimal Latitude { get; set; }
	}
}
