using MediatR;
using Application.Features.Etablissements.DTOs;

namespace Application.Features.Etablissements.Queries.GetByVille
{
    public class GetByVilleQuery : IRequest<List<EtablissementDto>>
    {
        public string Ville { get; set; } = string.Empty;
    }
}