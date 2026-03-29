using System;
using System.Threading;
using MediatR;
using Application.Features.Clients.Interfaces;
using Domain.Entities;

namespace Application.Features.Clients.Commands.CreateClient
{
    public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, Guid>
    {
        private readonly IClientRepository _clientRepository;

        public CreateClientCommandHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<Guid> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            // ✅ Valider Telephone et Ville avant de créer le client
            var client = new Client(
                request.Nom,
                request.Email,
                request.Telephone ?? throw new Exception("Le téléphone est obligatoire."),
                request.Ville     ?? throw new Exception("La ville est obligatoire."),
                request.Quartier    ?? throw new Exception("Le quartier  est obligatoire.")
            );

            await _clientRepository.AddAsync(client);
            return client.Id;
        }
    }
}