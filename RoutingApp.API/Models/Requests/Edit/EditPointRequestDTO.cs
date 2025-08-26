namespace RoutingApp.API.Models.DTO
{
	public class EditPointRequestDTO
	{
		public int Id { get; set; }
		public required string Name { get; set; }
		public required string Address { get; set; }
		public decimal Longitude { get; set; }
		public decimal Latitude { get; set; }
	}
}
