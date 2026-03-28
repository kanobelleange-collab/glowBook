using MediatR;
using Application.Features.Rendevou.Interfaces;
using Application.Features.Rendevou.DTOs;
using Domain.Entities;

namespace Application.Features.Rendevou.Queries.GetAll
{
    // Retourne une liste de RendezVousDto
    public class GetRendezVousClientQuery : IRequest<List<RendezVousDto>>
    {
        public Guid ClientId { get; set; }
        public StatutRendezVous? Statut { get; set; }
    }

}