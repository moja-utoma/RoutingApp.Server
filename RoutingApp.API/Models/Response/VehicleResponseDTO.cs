using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoutingApp.API.Models.DTO;

namespace RoutingApp.API.Models.Response
{
    public class VehicleResponseDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Capacity { get; set; }
        public int? WarehouseId { get; set; }
        public string? WarehouseName { get; set; }
        public string? WarehouseAddress { get; set; }
        //public WarehouseResponse? Warehouse { get; set; } // -
        // warehouse id
        //           name
        //           address

    }
}