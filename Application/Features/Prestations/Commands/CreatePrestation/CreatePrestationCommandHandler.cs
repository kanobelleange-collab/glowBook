// CreatePrestationCommand.cs

using Domain.Entities;
using Application.Features.Prestations.DTOs;
using MediatR;
using AutoMapper;
using  Application.Features.Prestations.Interfaces;
using Application.Features.Prestations.Commands.CreatePrestation;


namespace Appliccation.Features.Prestations.Commands.CreatePrestation
{
    


// Handler
public class CreatePrestationCommandHandler : IRequestHandler<CreatePrestationCommand, PrestationDto>
{
    private readonly IPrestationRepository _repository;
    private readonly IMapper _mapper;

    public CreatePrestationCommandHandler(IPrestationRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PrestationDto> Handle(CreatePrestationCommand request, CancellationToken cancellationToken)
    {
        var prestation = _mapper.Map<Prestation>(request);
        prestation.Id = Guid.NewGuid();

        await _repository.AddAsync(prestation);

        return _mapper.Map<PrestationDto>(prestation);
    }
}
}