using System;
using MediatR;

namespace Application.Features.Rendevou.Commands.AnnulerRendeVous
{
    public class AnnulerRendeVousCommand : IRequest
    {
        public Guid RendezVousId { get; set; }
        public string? Raison{ get; set; }
        public Guid ClientId {get ;set;}
    }
}