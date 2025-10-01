using Microsoft.AspNetCore.Mvc;
using RoutingApp.API.Services;
using RoutingApp.API.Services.Interfaces;
using System.Text.Json;

namespace RoutingApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrsController : ControllerBase
    {
        private readonly IOrsService _orsService;
        private readonly ILogger<OrsController> _logger;

        public OrsController(IOrsService orsService, ILogger<OrsController> logger)
        {
            _orsService = orsService;
            _logger = logger;
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

        //[HttpPost("stream/{routeId}")]
        //public async Task StreamRoute(string routeId, [FromBody] object payload)
        //{
        //    if (string.IsNullOrWhiteSpace(routeId) || payload == null || string.IsNullOrWhiteSpace(payload.ToString()))
        //    {
        //        Response.StatusCode = 400;
        //        await Response.WriteAsync("Route ID and payload are required.");
        //        return;
        //    }

        //    Response.ContentType = "text/event-stream";
        //    var cancellationToken = HttpContext.RequestAborted;

        //    try
        //    {
        //        await foreach (var coord in _orsService.StreamRouteAsync(routeId, payload.ToString()!, cancellationToken))
        //        {
        //            var timestamp = DateTime.UtcNow;
        //            var json = JsonSerializer.Serialize(new
        //            {
        //                routeId,
        //                lat = coord[1],
        //                lng = coord[0],
        //                timestamp = timestamp.ToString("o")
        //            });

        //            var ssePayload = $"id: {timestamp:O}\ndata: {json}\n\n";

        //            await Response.WriteAsync(ssePayload, cancellationToken);
        //            await Response.Body.FlushAsync(cancellationToken);

        //            _logger.LogInformation("Flushed SSE event for route {RouteId} at {Timestamp}: {Payload}", routeId, timestamp, json);
        //            await Task.Delay(500, cancellationToken);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var errorJson = JsonSerializer.Serialize(new { error = ex.Message });
        //        await Response.WriteAsync($"data: {errorJson}\n\n");
        //    }
        //}

        [HttpPost("stream/{routeId}")]
        public async Task StreamRoute(string routeId, [FromBody] object payload, [FromServices] IRouteStreamRegistry registry, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(routeId) || payload == null || string.IsNullOrWhiteSpace(payload.ToString()))
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Route ID and payload are required.");
                return;
            }

            Response.ContentType = "text/event-stream";
            //var cancellationToken = HttpContext.RequestAborted;

            registry.StartRouteStream(routeId, payload.ToString()!, cancellationToken);
            var channel = registry.GetOrCreateChannel(routeId);
            var reader = channel.Reader;

            try
            {
                while (await reader.WaitToReadAsync(cancellationToken))
                {
                    while (reader.TryRead(out var ssePayload))
                    {
                        await Response.WriteAsync(ssePayload, cancellationToken);
                        await Response.Body.FlushAsync(cancellationToken);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Client disconnected from route {RouteId}", routeId);
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
