using Application.Common.Interfaces;
using Application.Features.Etablissements.DTOs;
using Application.Features.Etablissements.Interfaces;
using MediatR;

namespace Application.Features.Etablissements.Queries.GetProximite
{
    public class GetProximiteQueryHandler
        : IRequestHandler<GetProximiteQuery, List<EtablissementDto>>
    {
        private readonly IEtablissementRepository _repository;
        private readonly IGeocodageService        _geocodage;

        public GetProximiteQueryHandler(
            IEtablissementRepository repository,
            IGeocodageService geocodage)
        {
            _repository = repository;
            _geocodage  = geocodage;
        }

        public async Task<List<EtablissementDto>> Handle(
            GetProximiteQuery query,
            CancellationToken cancellationToken)
        {
            var coords = await _geocodage.GeocodeAsync(
                query.Ville,
                query.Quartier)
                ?? throw new Exception(
                    $"Localisation introuvable : {query.Quartier}, {query.Ville}");

            return await _repository.GetProximiteAsync(
                coords.Latitude,
                coords.Longitude,
                query.RayonKm,
                query.TypeServiceNom);  // ← remplacé Categorie par TypeServiceNom
        }
    }
}