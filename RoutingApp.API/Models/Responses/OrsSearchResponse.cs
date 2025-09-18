namespace RoutingApp.API.Models.Responses
{
    public class OrsSearchResponse
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; } = "";
        public string FullAddress { get; set; } = "";
    }
}
