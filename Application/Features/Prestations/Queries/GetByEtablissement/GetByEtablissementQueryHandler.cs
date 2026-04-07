using MediatR;
using Application.Features.Prestations.Interfaces;
using Application.Features.Prestations.DTOs;
using AutoMapper;


namespace Application.Features.Prestations.Queries.GetByEtablissement
{
    


public class GetByEtablissementQueryHandler : IRequestHandler<GetByEtablissementQuery, List<PrestationDto>>
{
    private readonly IPrestationRepository _repository;
    private readonly IMapper _mapper;

    public GetByEtablissementQueryHandler(IPrestationRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<PrestationDto>> Handle(GetByEtablissementQuery request, CancellationToken cancellationToken)
    {
        var prestations = await _repository.GetByEtablissementAsync(request.EtablissementId);
        return _mapper.Map<List<PrestationDto>>(prestations);
    }
}
}