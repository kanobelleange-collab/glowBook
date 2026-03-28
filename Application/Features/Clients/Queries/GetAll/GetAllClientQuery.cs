using System;
using MediatR;
using Domain.Entities;
using Application.Features.Clients.DTOs;

namespace Application.Features.Clients.Queries.GetAll
{
    public class GetAllClientQuery : IRequest<List<ClientDto>>
    {
        
    }
}