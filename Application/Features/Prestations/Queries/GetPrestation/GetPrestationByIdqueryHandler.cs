// GetPrestationByIdQuery.cs
using MediatR;
using Application.Features.Prestations.Interfaces;
using Application.Features.Prestations.DTOs;
using AutoMapper;

namespace Application.Features.Prestations.Queries.GetPrestation
{
    


public class GetPrestationByIdQueryHandler : IRequestHandler<GetPrestationByIdQuery, PrestationDto?>
{
    private readonly IPrestationRepository _repository;
    private readonly IMapper _mapper;

    public GetPrestationByIdQueryHandler(IPrestationRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PrestationDto?> Handle(GetPrestationByIdQuery request, CancellationToken cancellationToken)
    {
        var prestation = await _repository.GetByIdAsync(request.Id);
        return prestation is null ? null : _mapper.Map<PrestationDto>(prestation);
    }
}
}