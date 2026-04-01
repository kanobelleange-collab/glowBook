using MediatR;
using Application.Features.Clients.DTOs;
namespace Application.Features.Clients.Commands.UpdateClient
{
    public record UpdateClientCommand : IRequest<ClientDto>
    {
        public Guid Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string Ville { get; set; } = string.Empty;
        public string Quartier { get; set; } = string.Empty;
        public DateTime DateInscription { get; set; }
        public bool EstActif { get; set; }
    }
}