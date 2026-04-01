using Application.Features.Etablissements.DTOs;
using MediatR;
using Application.Features.Etablissements.Interfaces;

namespace Application.Features.Etablissements.Queries.RechercherEtablissement
{
    public class RechercherQueryHandler
        : IRequestHandler<RechercherQuery, List<EtablissementDto>>
    {
        private readonly IEtablissementRepository _repository;

        public RechercherQueryHandler(IEtablissementRepository repository)
            => _repository = repository;

        public async Task<List<EtablissementDto>> Handle(
            RechercherQuery query,
            CancellationToken cancellationToken)
        {
            return await _repository.RechercherAsync(
                query.MotCle,
                query.Ville,
                query.TypeServiceNom);
        }
    }
}