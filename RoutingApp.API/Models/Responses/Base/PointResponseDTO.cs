namespace RoutingApp.API.Models.Responses.Base
{
	public class PointResponseDTO
	{
		public int Id { get; set; }
		public required string Name { get; set; }
		public required string Address { get; set; }
		public decimal Longitude { get; set; }
		public decimal Latitude { get; set; }
	}
}
