using System;
using MediatR;
using Application.Features.Clients.DTOs;

namespace Application.Features.Clients.Queries.GetById
{
    public class GetByIdClientQuery : IRequest<ClientDto>
    {
        public Guid Id { get; set; }
    }
}