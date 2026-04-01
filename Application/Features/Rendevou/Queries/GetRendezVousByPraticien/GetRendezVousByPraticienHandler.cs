using MediatR;
using Domain.Entities;
using Application.Features.Rendevou.DTOs;
using Application.Features.Rendevou.Interfaces;
using AutoMapper;
using Application.Features.Rendevou.GetRendezVousByPraticien;
namespace Application.Features.Rendevou.GetRendezVousByPraticien.GetRendezVousByPraticienHandler;
public class GetRendezVousByPraticienHandler : IRequestHandler<GetRendezVousByPraticienQuery, List<RendezVousDto>>
{
    private readonly IRendezVousRepository _repository;
    private readonly IMapper _mapper;

    public GetRendezVousByPraticienHandler(IRendezVousRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<RendezVousDto>> Handle(GetRendezVousByPraticienQuery request, CancellationToken ct)
    {
        var data = await _repository.GetByPraticienAsync(request.PraticienId);
        return _mapper.Map<List<RendezVousDto>>(data);
    }
}