using MediatR;
using Domain.Entities;
using Application.Features.Rendevou.DTOs;
using Application.Features.Rendevou.Interfaces;
using AutoMapper;
using Application.Features.Rendevou.GetRendezVousByEmployer;
namespace Application.Features.Rendevou.GetRendezVousByEmployer.GetRendezVousByEmployeeHandler;
public class GetRendezVousByEmployeeHandler : IRequestHandler<GetRendezVousByEmployeeQuery, List<RendezVousDto>>
{
    private readonly IRendezVousRepository _repository;
    private readonly IMapper _mapper;

    public GetRendezVousByEmployeeHandler(IRendezVousRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<RendezVousDto>> Handle(GetRendezVousByEmployeeQuery request, CancellationToken ct)
    {
        var data = await _repository.GetByEmployeeAsync(request.PraticienId);
        return _mapper.Map<List<RendezVousDto>>(data);
    }
}