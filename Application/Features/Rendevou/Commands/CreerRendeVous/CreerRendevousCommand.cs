using System;
using MediatR;
using Domain.Entities;
using Application.Features.Rendevou.DTOs;

namespace Application.Features.Rendevou.Commands.CreerRendeVous
{
    public class CreerRendeVousCommand : IRequest<RendezVousDto>
    {
        public Guid ClientId { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid ServiceId { get; set; }
        public Guid EtablissementId { get; set; }
        public DateTime DateHeure { get; set; }
        public string? NotesClient { get; set; }
    }
}