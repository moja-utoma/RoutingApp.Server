using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RoutingApp.API.Models.Request;
using RoutingApp.API.Services.Interfaces;

namespace RoutingApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        public VehiclesController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _vehicleService.GetAllVehiclesAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var result = await _vehicleService.GetVehicleByIDAsync(id);
            return Ok(result);
        }
        
        [HttpPost]
		public async Task<IActionResult> Create(CreateVehicleRequest dto)
		{
			try
			{
				var result = await _vehicleService.CreateVehicleAsync(dto);
				return Ok(result);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _vehicleService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Edit(EditVehicleRequest request)
        {
            try
            {
                var result = await _vehicleService.EditAsync(request);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}