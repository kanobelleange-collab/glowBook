using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Application.Features.Etablissements.DTOs;
using Domain.Enum;
namespace Application.Features.Etablissements.Commands.UpdateEtablissement
{
    public class UpdateEtablissementCommand : IRequest<EtablissementDto>
    {
        public Guid Id { get; set; } // ID de l'établissement à mettre à jour
        public required string Nom { get; set; }
        public required string Adresse { get; set; }
        public string Quartier  { get; set; } = string.Empty; // ✅ entré par l'établissement
        public required string Ville { get; set; }
        public  required string Telephone { get; set; }
        public  CategorieEtablissement Categorie { get; set; } // ✅ enum
            public string? Description { get; set; }
        public required string Email { get; set; }
        public  required string TypeEtablissement { get; set; }
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
    } // "SalonCoiffure", "CabinetProthetiste", "SalonMassage", "SpaBeaute"
    
}