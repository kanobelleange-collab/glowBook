using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Aviss.DTOs;
using MediatR;
using Application.Features.Aviss.Interfaces;
using AutoMapper;
using Application.Features.Aviss.Queries.GetAvisEtablissement;
namespace Application.Features.Aviss.Queries.GetAvisEtablissement
{
public class GetAvisEtablissementHandler : IRequestHandler<GetAvisEtablissementQuery, AvisEtablissementResponse>
{
    private readonly IAvisRepository _repository;
    private readonly IMapper _mapper;

    public GetAvisEtablissementHandler(IAvisRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AvisEtablissementResponse> Handle(GetAvisEtablissementQuery request, CancellationToken cancellationToken)
    {
        var avis = await _repository.GetByEtablissementAsync(request.EtablissementId);
        var moyenne = await _repository.CalculerNoteMoyenneAsync(request.EtablissementId);
        
        return new AvisEtablissementResponse(moyenne, _mapper.Map<List<AvisDto>>(avis));
    }
}
}