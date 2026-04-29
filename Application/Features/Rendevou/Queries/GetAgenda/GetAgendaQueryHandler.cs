// Application/Features/RendezVous/Queries/GetAgendaQueryHandler.cs

using MediatR;
using Application.Features.Rendevou.Interfaces;
using Application.Features.Rendevou.DTOs;

namespace Application.Features.RendeVou.Queries{


public class GetAgendaQueryHandler 
    : IRequestHandler<GetAgendaQuery, List<RendezVousDto>>
{
    private readonly IRendezVousRepository _repository;

    public GetAgendaQueryHandler(IRendezVousRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<RendezVousDto>> Handle(
        GetAgendaQuery request,
        CancellationToken cancellationToken)
    {
        return await _repository.GetAgendaAsync(
            request.EtablissementId,
            request.Date,
            cancellationToken);
    }
}
}