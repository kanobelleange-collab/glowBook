using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Domain.Entities;
using Application.Features.Rendevou.DTOs;
using Application.Features.Rendevou.Interfaces;
using AutoMapper;

using Application.Features.Rendevou.Queries.GetByStatus;

namespace Application.Features.Rendevou.Queries.GetByStatus.GetByStatusHandler
{
    public record GetRendezVousByStatutQuery(StatutRendezVous Statut) : IRequest<List<RendezVousDto>>;

public class GetRendezVousByStatutHandler : IRequestHandler<GetRendezVousByStatutQuery, List<RendezVousDto>>
{
    private readonly IRendezVousRepository _repository;
    private readonly IMapper _mapper;

    public GetRendezVousByStatutHandler(IRendezVousRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<RendezVousDto>> Handle(GetRendezVousByStatutQuery request, CancellationToken ct)
    {
        var result = await _repository.GetByStatutAsync(request.Statut);
        return _mapper.Map<List<RendezVousDto>>(result);
    }
}
}