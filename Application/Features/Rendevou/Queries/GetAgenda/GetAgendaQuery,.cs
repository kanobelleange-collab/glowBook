// Application/Features/RendezVous/Queries/GetAgendaQuery.cs
using MediatR;
using Domain.Entities;
using Application.Features.Rendevou.DTOs;

namespace Application.Features.RendeVou.Queries{

public class GetAgendaQuery : IRequest<List<RendezVousDto>>
{
    public Guid EtablissementId { get; set; }
    public DateTime Date { get; set; }
}
}