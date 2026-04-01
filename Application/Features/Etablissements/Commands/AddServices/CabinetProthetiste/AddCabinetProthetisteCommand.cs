using MediatR;
using Domain.Entities;
using Application.Features.Etablissements.DTOs;

namespace Application.Features.Etablissements.Commands.AddServices.CabinetProthetiste
{
    public class AddCabinetProthetisteCommand : IRequest<Guid>
    {
        public Guid EtablissementId { get; set; }
        public bool ProposeProtheseOngles { get; set; }
        public bool ProposeExtensionCils { get; set; }
        public bool ProposeProtheseCapillaire { get; set; }
        public List<string> MatieresOngles { get; set; } = new();
        public List<string> StylesCils { get; set; } = new();
        public List<PrestationDto> Prestations { get; set; } = new();
    }
}