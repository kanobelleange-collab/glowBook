using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using Application.Features.Rendevou.Queries.GetByEmployeeDate;
using Application.Features.Rendevou.Interfaces;
using Application.Features.Rendevou.DTOs;
namespace Application.Features.Rendevou.Queries.GetByEmployeeDate.GetRendezVousByEmployeeDateHandler
{
    public class GetRendezVousByEmployeeDateHandler : IRequestHandler<GetRendezVousByEmployeeDateQuery, List<RendezVousDto>>
{
    private readonly IRendezVousRepository _repository;
    private readonly IMapper _mapper;

    public GetRendezVousByEmployeeDateHandler(IRendezVousRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<RendezVousDto>> Handle(GetRendezVousByEmployeeDateQuery request, CancellationToken ct)
    {
        var data = await _repository.GetByEmployeeEtDateAsync(request.PraticienId, request.Date);
        return _mapper.Map<List<RendezVousDto>>(data);
    }
}
}