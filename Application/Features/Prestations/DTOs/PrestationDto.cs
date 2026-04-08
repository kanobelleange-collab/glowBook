using Domain.Entities;


namespace Application.Features.Prestations.DTOs
{
    public class PrestationDto
    {
        public Guid ServiceId { get; set; }
        public Guid Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Prix { get; set; }
        public int DureeMinutes { get; set; }
    

        // ✅ Propriétés calculées — utiles pour l'affichage
        public string DureeAffichage =>
            DureeMinutes >= 60
                ? $"{DureeMinutes / 60}h{(DureeMinutes % 60 > 0 ? $"{DureeMinutes % 60}min" : "")}"
                : $"{DureeMinutes}min";
        // Exemple : 90 min → "1h30min", 60 min → "1h", 30 min → "30min"

        public string PrixAffichage => $"{Prix:N0} FCFA";
        // Exemple : 5000 → "5 000 FCFA"
    }
}