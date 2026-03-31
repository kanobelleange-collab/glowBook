
using Application.Features.Etablissements.DTOs;
using MediatR;
using Application.Features.Etablissements.Interfaces;
using AutoMapper;


namespace Application.Features.Etablissements.Queries.GetMieuxNotes
{
   

    public class GetMieuxNotesQueryHandler
        : IRequestHandler<GetMieuxNotesQuery, List<EtablissementDto>>
    {
        private readonly IEtablissementRepository _repository;
        private readonly IMapper _mapper;

        public GetMieuxNotesQueryHandler(
            IEtablissementRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper     = mapper;
        }

        public async Task<List<EtablissementDto>> Handle(
            GetMieuxNotesQuery query,
            CancellationToken cancellationToken)
        {
            var etablissements = await _repository
                .GetMieuxNotesAsync(query.Top);

            return _mapper.Map<List<EtablissementDto>>(etablissements);
        }
    }
}