using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoutingApp.API.Data;
using RoutingApp.API.Data.Entities;
using RoutingApp.API.Models;
using RoutingApp.API.Models.DTO;
using RoutingApp.API.Services.Interfaces;

namespace RoutingApp.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class DeliveryPointsController : Controller
	{
		private readonly IDeliveryPointService _pointService;
        private readonly ILogger<DeliveryPointsController> _logger;

        public DeliveryPointsController(IDeliveryPointService pointService, ILogger<DeliveryPointsController> logger)
		{
			_pointService = pointService;
			_logger = logger; 
		}

		[HttpGet]
		public async Task<IActionResult> GetAll([FromQuery] QueryParametersModel filters)
		{
			try
			{
				var result = await _pointService.GetAllPointsAsync(filters);
				return Ok(result);
			}
			catch (Exception e)
            {
                _logger.LogError(e, "Encountered error while getting delivery points: {e.Message}", e.Message);
                return BadRequest(e.Message);
            }
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetByID(int id)
		{
			var result = await _pointService.GetPointByIDAsync(id);
			return Ok(result);
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreateDeliveryPointRequestDTO dto)
		{
			try
			{
				var result = await _pointService.CreatePointAsync(dto);
				return Ok(result);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[HttpPost("File")]
		public async Task<IActionResult> ImportCSV(IFormFile file)
		{
			try
			{
				var result = await _pointService.ImportCSV(file);
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
				await _pointService.DeleteAsync(id);
				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[HttpPut]
		public async Task<IActionResult> Edit(EditDeliveryPointRequestDTO request)
		{
			try
			{
				var result = await _pointService.EditAsync( request);
				return Ok(result);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}
