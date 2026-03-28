
using MediatR;
using Application.Features.ServiceEsthetiques.DTOs;

namespace Application.Features.ServiceEsthetiques.Queries.GetAll.GetSeviceByEtablissement
{
    public class GetServicesByEtablissementQuery : IRequest<List<ServiceEsthetiqueDto>>
    {
        public Guid EtablissementId { get; set; }
        public bool DisponiblesSeulement { get; set; } = false;
    }

    
}