
using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Common.Interfaces;

namespace Infrastructure.Services
{
    public class GeocodageService : IGeocodageService
    {
        private readonly HttpClient _httpClient;

        public GeocodageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CoordonnéesGps?> GeocodeAsync(
            string ville, string? quartier = null)
        {
            try
            {
                var adresse = quartier != null
                    ? $"{quartier}, {ville}, Cameroun"
                    : $"{ville}, Cameroun";

                var url = "https://nominatim.openstreetmap.org/search" +
                          $"?q={Uri.EscapeDataString(adresse)}" +
                          "&format=json&limit=1";

                var response = await _httpClient.GetStringAsync(url);
                var results  = JsonSerializer
                    .Deserialize<List<NominatimResult>>(response);

                if (results == null || !results.Any())
                    return null;

                var first = results.First();
                return new CoordonnéesGps
                {
                    Latitude = double.Parse(first.Lat,
                        System.Globalization.CultureInfo.InvariantCulture),
                    Longitude = double.Parse(first.Lon,
                        System.Globalization.CultureInfo.InvariantCulture),
                    AdresseComplete = first.DisplayName
                };
            }
            catch
            {
                return null;
            }
        }
    }

    public class NominatimResult
    {
        [JsonPropertyName("lat")]
        public string Lat { get; set; } = string.Empty;

        [JsonPropertyName("lon")]
        public string Lon { get; set; } = string.Empty;

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;
    }
}