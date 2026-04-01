using MediatR;
using Domain.Entities;

namespace Application.Features.Clients.Commands.CreateClient
{
    public record CreateClientCommand : IRequest<Client>
    {
        public string Nom { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string Ville { get; set; } = string.Empty;
        public string Quartier { get; set; } = string.Empty;
        public DateTime DateInscription { get; set; }
        public bool EstActif { get; set; }
    }
}