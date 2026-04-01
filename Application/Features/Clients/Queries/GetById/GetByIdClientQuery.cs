using MediatR;
using Application.Features.Clients.DTOs;

namespace Application.Features.Clients.Queries.GetById
{
    public record GetByIdClientQuery(Guid Id) : IRequest<ClientDto?>;
}