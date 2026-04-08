// GetByServiceQuery.cs
using MediatR;
using AutoMapper;
using Application.Features.Prestations.DTOs;
using Application.Features.Prestations.Interfaces;

namespace Application.Features.Prestations.Queries.GetServices
{
    


public class GetByServiceQueryHandler : IRequestHandler<GetByServiceQuery, List<PrestationDto>>
{
    private readonly IPrestationRepository _repository;
    private readonly IMapper _mapper;

    public GetByServiceQueryHandler(IPrestationRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<PrestationDto>> Handle(GetByServiceQuery request, CancellationToken cancellationToken)
    {
        var prestations = await _repository.GetByServiceAsync(request.ServiceId);
        return _mapper.Map<List<PrestationDto>>(prestations);
    }
}
}