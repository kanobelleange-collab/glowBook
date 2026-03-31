


namespace Application.Features.Etablissements.DTOs
{
    public class PrestationDto
    {
        public string Nom { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Prix { get; set; }
        public int DureeMinutes { get; set; }
    }
}