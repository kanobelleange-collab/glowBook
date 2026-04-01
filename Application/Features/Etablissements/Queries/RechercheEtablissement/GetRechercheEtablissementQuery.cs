using MediatR;
using Application.Features.Etablissements.DTOs;

namespace Application.Features.Etablissements.Queries.RechercherEtablissement
{
    public class RechercherQuery : IRequest<List<EtablissementDto>>
    {
        public string? MotCle { get; set; }
        public string? Ville { get; set; }
        public string? TypeServiceNom { get; set; }
    }
}