using RoutingApp.API.Models.Responses;
using RoutingApp.API.Services.Interfaces;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace RoutingApp.API.Services
{
    public class OrsService : IOrsService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey;
        private const string BaseUrl = "https://api.openrouteservice.org";

        public OrsService(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _apiKey = config["OpenRouteService:ApiKey"]
                      ?? throw new InvalidOperationException("API key not found");
        }

        public async Task<OrsSearchResponse?> SearchAddressAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return null;

            var encoded = Uri.EscapeDataString(query);
            var layers = "address,venue";
            var url = $"{BaseUrl}/geocode/search?api_key={_apiKey}&text={encoded}&layers={layers}";

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonDocument.Parse(json);

            if (!data.RootElement.TryGetProperty("features", out var features) || features.GetArrayLength() == 0)
                return null;

            return ParseBestFeature(features);
        }

        public async Task<OrsSearchResponse?> ReverseSearchAsync(double lat, double lng)
        {
            var layers = "address,venue";
            var url = $"{BaseUrl}/geocode/reverse?api_key={_apiKey}&point.lat={lat}&point.lon={lng}&layers={layers}";

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonDocument.Parse(json);

            if (!data.RootElement.TryGetProperty("features", out var features) || features.GetArrayLength() == 0)
                return null;

            return ParseBestFeature(features);
        }

        private OrsSearchResponse? ParseBestFeature(JsonElement features)
        {
            var bestFeature = features.EnumerateArray()
                .Where(f =>
                {
                    var props = f.GetProperty("properties");
                    var layer = props.TryGetProperty("layer", out var l) ? l.GetString() : "";
                    var confidence = props.TryGetProperty("confidence", out var c) ? c.GetDouble() : 0;
                    return confidence >= 0.6 && (layer == "address" || layer == "venue");
                })
                .OrderByDescending(f => f.GetProperty("properties").GetProperty("confidence").GetDouble())
                .FirstOrDefault();

            bestFeature = bestFeature.ValueKind != JsonValueKind.Undefined ? bestFeature : features[0];

            var coords = bestFeature.GetProperty("geometry").GetProperty("coordinates");
            var props = bestFeature.GetProperty("properties");

            var latitude = coords[1].GetDouble();
            var longitude = coords[0].GetDouble();

            var street = props.TryGetProperty("street", out var s) ? s.GetString() : "";
            var number = props.TryGetProperty("housenumber", out var n) ? n.GetString() : "";
            var city = props.TryGetProperty("locality", out var c) ? c.GetString() :
                       props.TryGetProperty("region", out var r) ? r.GetString() : "";

            var shortAddress = string.Join(", ", new[] { street, number, city }.Where(x => !string.IsNullOrWhiteSpace(x)));
            var fullAddress = props.TryGetProperty("label", out var label) ? label.GetString() : shortAddress;

            return new OrsSearchResponse
            {
                Latitude = latitude,
                Longitude = longitude,
                Address = shortAddress,
                FullAddress = fullAddress
            };
        }

        public async Task<string?> GetRouteAsync(string payloadJson)
        {
            if (string.IsNullOrWhiteSpace(payloadJson)) return null;

            var client = _httpClientFactory.CreateClient();
            var url = $"{BaseUrl}/v2/directions/driving-car/geojson";

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(payloadJson, Encoding.UTF8, "application/json")
            };

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"ORS request failed: {content}");

            return content;
        }
    }
}
