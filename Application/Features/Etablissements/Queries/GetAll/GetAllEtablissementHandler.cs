using System;
using MediatR;
using AutoMapper;
using  Application.Features.Etablissements.DTOs;
using Application.Features.Etablissements.Interfaces;


namespace Application.Features.Etablissements.Queries.GetAll
{
    public class GetAllEtablissementHandler : IRequestHandler<GetAllEtablissementQuery, List<EtablissementDto>>
    {
        private readonly IEtablissementRepository _etablissementRepository;
        private readonly IMapper _mapper;

        public GetAllEtablissementHandler(IEtablissementRepository etablissementRepository, IMapper mapper)
        {
            _etablissementRepository = etablissementRepository;
            _mapper = mapper;
        }

        public async Task<List<EtablissementDto>> Handle(GetAllEtablissementQuery request, CancellationToken cancellationToken)
        {
            var etablissements = await _etablissementRepository.GetAllAsync();
            return _mapper.Map<List<EtablissementDto>>(etablissements);
        }
    }
}