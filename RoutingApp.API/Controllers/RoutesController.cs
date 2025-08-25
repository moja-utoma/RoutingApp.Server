using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoutingApp.API.Models.DTO;
using RoutingApp.API.Services.Interfaces;
using RoutingApp.API.Validation;
using System.Threading.Tasks;

namespace RoutingApp.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class RoutesController : Controller
	{
		private readonly IRouteService _routeService;
        private readonly ILogger<RoutesController> _logger;

        public RoutesController(IRouteService routeService, ILogger<RoutesController> logger)
		{
			_routeService = routeService;
			_logger = logger;

            _logger.LogInformation("RoutesController Started");
        }

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var result = await _routeService.GetAllRoutesAsync();
			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetByID(int id)
		{
			try
			{
				var result = await _routeService.GetRouteByIDAsync(id);
				return Ok(result);
			}
			catch (Exception e)
            {
                _logger.LogError(e, "Encountered error while getting route by id: {e.Message}", e.Message);
                return BadRequest(e.Message);
            }
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreateRouteRequest dto)
		{
			try
			{
				var validator = new RouteValidator();
				await validator.ValidateAndThrowAsync(dto);

				var result = await _routeService.CreateRouteAsync(dto);
				return Ok(result);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Encountered error while creating route: {e.Message}", e.Message);
				return BadRequest(e.Message);
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				await _routeService.DeleteAsync(id);
				return Ok();
			}
			catch (Exception e)
			{
                _logger.LogError(e, "Encountered error while deleting route: {e.Message}", e.Message);
                return BadRequest(e.Message);
			}
		}

		[HttpPut]
		public async Task<IActionResult> Edit(EditRouteRequest request)
		{
			try
			{
				var result = await _routeService.EditAsync(request);
				return Ok(result);
			}
			catch (Exception e)
			{
                _logger.LogError(e, "Encountered error while updating route: {e.Message}", e.Message);
                return BadRequest(e.Message);
			}
		}
	}
}
