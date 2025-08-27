using RoutingApp.API.Models.Responses.Base;

namespace RoutingApp.API.Models.Responses.DeliveryPoints
{
    public class DeliveryPointDetailsResponseDTO : PointResponseDTO
    {
        public decimal Weight { get; set; }
    }
}
