using MediatR;
using Domain.Entities;
using Application.Features.Etablissements.DTOs;

namespace Application.Features.Etablissements.Commands.AddServices.CabinetEsthetique
{
    public class AddCabinetEsthetiqueCommand : IRequest<Guid>
    {
        public Guid EtablissementId { get; set; }
        public bool ProposeSoinsVisage { get; set; }
        public bool ProposeEpilation { get; set; }
        public bool ProposeOnglerie { get; set; }
        public bool ProposeMaquillage { get; set; }
        public List<string> TechniquesEpilation { get; set; } = new();
        public List<string> TypesSoinsVisage { get; set; } = new();
        // public List<PrestationEtablissementsDto> Prestations { get; set; } = new();
    }
}