using MediatR;
using Domain.Entities;
using Application.Features.Etablissements.DTOs;

namespace Application.Features.Etablissements.Commands.AddServices.SalonCoiffure
{
    public class AddSalonCoiffureCommand : IRequest<Guid>
    {
        public Guid EtablissementId { get; set; }

        // ← AJOUT du TypeServiceNom fixe pour ce type de service
        public string TypeServiceNom { get; set; } =string.Empty;
        
        public List<string> SpecialitesTresse { get; set; } = new();
        public List<string> TypesCheveux { get; set; } = new();
        public bool AccepteHommes { get; set; } = false;
        public bool AccepteEnfants { get; set; } = false;
        public List<PrestationDto> Prestations { get; set; } = new();
    }
}