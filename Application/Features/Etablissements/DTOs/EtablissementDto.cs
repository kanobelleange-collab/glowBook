using System;

namespace Application.Features.Etablissements.DTOs
{
    public class EtablissementDto
    {
        public Guid Id { get; set; }
        public required string Nom { get; set; }
        public required  string Adresse { get; set; }

        public required string  Quartier{get;set;} // ✅ entré par l'établissement
        public  required string Ville { get; set; }
        public required  string Telephone { get; set; }
         public CategorieEtablissement Categorie { get; set; } // ✅ enum
         public string? Description { get; set; }
        public  required string Email { get; set; }

        public List<string>? Photos { get; set; } // URLs des photos "SalonCoiffure", "CabinetProthetiste", "SalonMassage", "SpaBeaute"
        public List<HoraireOuvertureDto>? Horaires { get; set; } //
   
        public required string TypeEtablissement { get; set; }
        public double? DistanceKm { get; set; } // "SalonCoiffure", "CabinetProthetiste", "SalonMassage", "SpaBeaute"
    }
}