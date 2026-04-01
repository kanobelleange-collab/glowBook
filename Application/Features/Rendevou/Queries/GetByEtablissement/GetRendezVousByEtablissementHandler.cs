using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Application.Features.Rendevou.DTOs;
using Application.Features.Rendevou.Interfaces;
using AutoMapper;
using Application.Features.Rendevou.Queries.GetByEtablissement;

namespace Application.Features.Rendevou.Queries.GetByEtablissement.GetRendezVousByEtablissementHandler
{
    public class GetRendezVousByEtablissementHandler : IRequestHandler<GetRendezVousByEtablissementQuery, List<RendezVousDto>>
{
    private readonly IRendezVousRepository _repository;
    private readonly IMapper _mapper;

    public GetRendezVousByEtablissementHandler(IRendezVousRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<RendezVousDto>> Handle(GetRendezVousByEtablissementQuery request, CancellationToken ct)
    {
        var data = await _repository.GetByEtablissementAsync(request.EtablissementId);
        return _mapper.Map<List<RendezVousDto>>(data);
    }
}
}