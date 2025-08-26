using RoutingApp.API.Models.Response.GetAll;

namespace RoutingApp.API.Models.Response.GetByID
{
    public class DetailsDeliveryPointResponseDTO : DetailsPointResponseDTO
    {
        public decimal Weight { get; set; }
    }
}
