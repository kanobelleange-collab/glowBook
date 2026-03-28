using System;
using MediatR;
using Application.Features.Etablissements.DTOs;
using Domain.Entities;
using AutoMapper;
using Application.Features.Etablissements.Interfaces;

namespace Application.Features.Etablissements.Queries.GetById
{
    public class GetByIdEtablissementHandler : IRequestHandler<GetByIdEtablissementQuery, EtablissementDto>
    {
        private readonly IEtablissementRepository _etablissementRepository;
        private readonly IMapper _mapper;

        public GetByIdEtablissementHandler(IEtablissementRepository etablissementRepository, IMapper mapper)
        {
            _etablissementRepository = etablissementRepository;
            _mapper = mapper;
        }

        public async Task<EtablissementDto> Handle(GetByIdEtablissementQuery request, CancellationToken cancellationToken)
        {
            var etablissement = await _etablissementRepository.GetByIdAsync(request.Id);
            return _mapper.Map<EtablissementDto>(etablissement);
        }
    }
}