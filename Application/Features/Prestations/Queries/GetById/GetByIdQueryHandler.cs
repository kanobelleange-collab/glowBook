using MediatR;
using AutoMapper;
using Application.Features.Prestations.DTOs;
using Application.Features.Prestations.Interfaces;
using Domain.Entities;


namespace Application.Features.Prestations.Queries.GetById
{
    


public class GetPrestationByIdQueryHandler 
    : IRequestHandler<GetPrestationByIdQuery, Prestation?>
{
    private readonly IPrestationRepository _repository;
    private readonly IMapper _mapper;

    public GetPrestationByIdQueryHandler(
        IPrestationRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Prestation?> Handle(
        GetPrestationByIdQuery request,
        CancellationToken cancellationToken)
    {
        var prestation = await _repository.GetByIdAsync(request.Id);

        if (prestation == null)
            return null;

        return _mapper.Map<Prestation>(prestation);
    }
}
}