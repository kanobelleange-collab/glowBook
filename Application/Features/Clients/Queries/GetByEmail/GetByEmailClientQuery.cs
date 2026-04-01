using MediatR;
using Application.Features.Clients.DTOs;

namespace Application.Features.Clients.Queries.GetClientByEmail
{
    // On passe l'email en paramètre et on attend un ClientDto (ou null)
    public record GetClientByEmailQuery(string Email) : IRequest<ClientDto?>;
}