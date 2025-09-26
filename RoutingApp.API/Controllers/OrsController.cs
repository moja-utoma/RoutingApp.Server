using Microsoft.AspNetCore.Mvc;
using RoutingApp.API.Services.Interfaces;
using System.Text.Json;

namespace RoutingApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrsController : ControllerBase
    {
        private readonly IOrsService _orsService;

        public OrsController(IOrsService orsService)
        {
            _orsService = orsService;
        }

        [HttpPost("route")]
        public async Task<IActionResult> GetRoute([FromBody] object payload)
        {
            if (payload == null || string.IsNullOrWhiteSpace(payload.ToString()))
                return BadRequest("Payload is required.");

            try
            {
                var geoJson = await _orsService.GetRouteAsync(payload.ToString()!);
                return Ok(geoJson);
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("stream")]
        public async Task StreamRoute([FromBody] object payload)
        {
            if (payload == null || string.IsNullOrWhiteSpace(payload.ToString()))
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Payload is required.");
                return;
            }

            Response.ContentType = "text/event-stream";

            try
            {
                await foreach (var coord in _orsService.StreamRouteAsync(payload.ToString()!))
                {
                    var json = JsonSerializer.Serialize(new { lat = coord[1], lng = coord[0] });
                    await Response.WriteAsync($"data: {json}\n\n");
                    await Response.Body.FlushAsync();
                    await Task.Delay(500); // simulate movement delay
                }
            }
            catch (Exception ex)
            {
                await Response.WriteAsync($"data: {{ \"error\": \"{ex.Message}\" }}\n\n");
            }
        }


        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return BadRequest("Query text is required.");

            var result = await _orsService.SearchAddressAsync(text);
            if (result == null)
                return NotFound("No results found.");

            return Ok(result);
        }

        [HttpGet("reverse")]
        public async Task<IActionResult> ReverseSearch([FromQuery] double lat, [FromQuery] double lng)
        {
            var result = await _orsService.ReverseSearchAsync(lat, lng);
            if (result == null)
                return NotFound("No address found.");

            return Ok(result);
        }
    }
}
