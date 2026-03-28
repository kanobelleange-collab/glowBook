using MediatR;
using Application.Features.Praticiens.DTOs;

namespace Application.Features.Praticiens.Commands.AjoutDisponibilite
{
    public class AjouterDisponibiliteCommand : IRequest<PraticienDto>
    {
        public Guid PraticienId { get; set; }
        public  required string Jour { get; set; }
        // ex : "Monday", "Tuesday"...
        public required  string HeureDebut { get; set; }
        // ex : "09:00"
        public required string HeureFin { get; set; }
        // ex : "18:00"
    }
}