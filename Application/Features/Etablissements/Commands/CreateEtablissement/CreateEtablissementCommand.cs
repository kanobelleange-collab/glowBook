using MediatR;
using Application.Features.Etablissements.DTOs;
using Domain.Enum;
 


namespace Application.Features.Etablissements.Commands.CreateEtablissement
{
    public class CreateEtablissementCommand : IRequest<EtablissementDto>
    {
        // Propriétés communes à tous les établissements
        public  required string TypeEtablissement { get; set; }
        public  required string Nom { get; set; }
        public  required string Adresse { get; set; }
        public required string Quartier {get;set;}
        public  required string Ville { get; set; }
        public  required string Telephone { get; set; }
        public  required string Email { get; set; }
        public List<string>? Photos { get; set; }
        public List<HoraireOuvertureDto>? Horaires { get; set; }

        // -------------------------------------------------------
        // Propriétés spécifiques SalonCoiffure
        // -------------------------------------------------------
        public List<string>? SpecialitesTresse { get; set; }
        // ex: ["BoxBraid", "Locks", "Tissage"]
        public List<string>? TypesCheveux { get; set; }
        // ex: ["Naturels", "Défrisés", "Métissés"]
        public bool AccepteHommes { get; set; } = false;
        public bool AccepteEnfants { get; set; } = false;

        // -------------------------------------------------------
        // Propriétés spécifiques SalonMassage
        // -------------------------------------------------------
        public List<string>? TypesMassage { get; set; }
        // ex: ["Suédois", "Thaï", "Pierre chaude"]
        public string? Ambiance { get; set; }
        // ex: "Zen", "Sportif", "Relaxant"
        public bool DisponibleADomicile { get; set; } = false;
        public int DureeMinimaleEnMinutes { get; set; } = 30;

        // -------------------------------------------------------
        // Propriétés spécifiques CabinetEsthetique
        // -------------------------------------------------------
        public bool ProposeSoinsVisage { get; set; } = false;
        public bool ProposeEpilation { get; set; } = false;
        public bool ProposeOnglerie { get; set; } = false;
        public bool ProposeMaquillage { get; set; } = false;

        // -------------------------------------------------------
        // Propriétés spécifiques CabinetProthetiste
        // -------------------------------------------------------
        public bool ProposeProtheseOngles { get; set; } = false;
        public bool ProposeExtensionCils { get; set; } = false;
        public bool ProposeProtheseCapillaire { get; set; } = false;

        // -------------------------------------------------------
        // Propriétés spécifiques SpaBeaute
        // -------------------------------------------------------
        public ServiceSpa Service{get;set;}
        // ex: ServiceSpa.Hammam | ServiceSpa.Sauna | ServiceSpa.Jacuzzi
    }
}