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
	public class WarehousesController : Controller
	{
		private readonly IWarehouseService _pointService;

		public WarehousesController(IWarehouseService pointService)
		{
			_pointService = pointService;
		}

		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] QueryParametersModel filters)
		{
			var result = await _pointService.GetAllPointsAsync(filters);
			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetByID(int id)
		{
			var result = await _pointService.GetPointByIDAsync(id);
			return Ok(result);
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreateWarehouseRequestDTO dto)
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
		public async Task<IActionResult> Edit(EditWarehouseRequestDTO request)
		{
			try
			{
				var result = await _pointService.EditAsync(request);
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
	}
}
