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
            var client = _mapper.Map<Client>(request);
            
            await _repository.AddAsync(client);
            
            return client; 
        }
    }
}