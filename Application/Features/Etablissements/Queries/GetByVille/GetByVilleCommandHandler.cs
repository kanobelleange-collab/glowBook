// Application/Features/Etablissements/Queries/GetByVille/GetByVilleQuery.cs
using Application.Features.Etablissements.DTOs;
using MediatR;
using Application.Features.Etablissements.Interfaces;
using  AutoMapper;  


namespace Application.Features.Etablissements.Queries.GetByVille
{
   
    public class GetByVilleQueryHandler
        : IRequestHandler<GetByVilleQuery, List<EtablissementDto>>
    {
        private readonly IEtablissementRepository _repository;
        private readonly IMapper _mapper;

        public GetByVilleQueryHandler(
            IEtablissementRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper     = mapper;
        }

        public async Task<List<EtablissementDto>> Handle(
            GetByVilleQuery query,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(query.Ville))
                throw new ArgumentException("La ville est obligatoire.");

            var etablissements = await _repository.GetByVilleAsync(query.Ville);

            if (!etablissements.Any())
                throw new Exception($"Aucun établissement trouvé à {query.Ville}.");

            return _mapper.Map<List<EtablissementDto>>(etablissements);
        }
    }
}