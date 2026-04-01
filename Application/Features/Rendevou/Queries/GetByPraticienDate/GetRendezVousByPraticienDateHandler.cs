using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using Application.Features.Rendevou.Queries.GetByPraticienDate;
using Application.Features.Rendevou.Interfaces;
using Application.Features.Rendevou.DTOs;
namespace Application.Features.Rendevou.Queries.GetByPraticienDate.GetRendezVousByPraticienDateHandler
{
    public class GetRendezVousByPraticienDateHandler : IRequestHandler<GetRendezVousByPraticienDateQuery, List<RendezVousDto>>
{
    private readonly IRendezVousRepository _repository;
    private readonly IMapper _mapper;

    public GetRendezVousByPraticienDateHandler(IRendezVousRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<RendezVousDto>> Handle(GetRendezVousByPraticienDateQuery request, CancellationToken ct)
    {
        var data = await _repository.GetByPraticienEtDateAsync(request.PraticienId, request.Date);
        return _mapper.Map<List<RendezVousDto>>(data);
    }
}
}