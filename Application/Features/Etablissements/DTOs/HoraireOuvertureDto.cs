
namespace Application.Features.Etablissements.DTOs
{
    

 public class HoraireOuvertureDto
    {
        public required string Jour { get; set; }           // "Monday", "Tuesday", etc.
        public required string HeureOuverture { get; set; } // "09:00"
        public required string HeureFermeture { get; set; } // "18:00"
    }
}