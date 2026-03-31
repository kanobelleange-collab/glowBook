using MediatR;
using Domain.Entities;
using Application.Features.Etablissements.DTOs;


namespace Application.Features.Etablissements.Commands.AddServices.SalonCoiffure
{
    public class AddSalonCoiffureCommand : IRequest<Guid>
    {
        public Guid EtablissementId { get; set; }
        public List<string> SpecialitesTresse { get; set; } = new();
        public List<string> TypesCheveux { get; set; } = new();
        public bool AccepteHommes { get; set; } = false;
        public bool AccepteEnfants { get; set; } = false;
        public List<PrestationDto> Prestations { get; set; } = new();
    }
}