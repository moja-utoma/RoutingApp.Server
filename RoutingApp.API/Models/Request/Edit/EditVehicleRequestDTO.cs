using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoutingApp.API.Models.Request
{
    public class EditVehicleRequestDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Capacity { get; set; }
        public int? WarehouseId { get; set; }
    }
}