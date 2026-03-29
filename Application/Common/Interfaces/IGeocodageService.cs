using System;

namespace Application.Common.Interfaces
{
    public class CoordonnéesGps
    {
        public double Latitude       { get; set; }
        public double Longitude      { get; set; }
        public string AdresseComplete { get; set; } = string.Empty;
    }

    public interface IGeocodageService
    {
        Task<CoordonnéesGps?> GeocodeAsync(string ville, string? quartier = null);
    }
}