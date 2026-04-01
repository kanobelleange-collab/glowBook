using AutoMapper;
using Domain.Entities;
using Application.Features.Clients.Interfaces;
using MediatR;

namespace Application.Features.Clients.Commands.CreateClient
{
    public class CreateClientHandler : IRequestHandler<CreateClientCommand, Client>
    {
        private readonly IClientRepository _repository;
        private readonly IMapper _mapper;

        public CreateClientHandler(IClientRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Client> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            // 1. On mappe le DTO vers l'entité
            var client = _mapper.Map<Client>(request);
            
            // 2. GÉNÉRATION DES DONNÉES MANQUANTES (C'est ici que ça se jouait !)
            client.Id = Guid.NewGuid();                // Génère un ID unique pour MySQL
            client.DateInscription = DateTime.Now;     // Date actuelle (Yaoundé/Douala)
            client.EstActif = true;                    // Par défaut, le client est actif

            // 3. On envoie l'objet complet au Repository
            await _repository.AddAsync(client);
            
            return client; 
        }
    }
}