using System;
using Application.Features.Clients.DTOs;
using Application.Features.Clients.Interfaces;
using MediatR;
using Application.Features.Clients.Queries.GetAll;

namespace Application.Features.Clients.Queries.GetAll
{
    public class GetAllClientHandler : IRequestHandler<GetAllClientQuery, List<ClientDto>>
    {
        private readonly IClientRepository _clientRepository;

        public GetAllClientHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<List<ClientDto>> Handle(GetAllClientQuery request, CancellationToken cancellationToken)
        {
            var clients = await _clientRepository.GetAllAsync();
            return clients.Select(c => new ClientDto
            {
                // Id = c.Id,
                Nom = c.Nom,
                Email = c.Email,
                Telephone = c.Telephone,
                Ville = c.Ville,
                
            }).ToList();
        }
    }
}