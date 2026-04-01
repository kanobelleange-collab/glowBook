using AutoMapper;
using Application.Features.Clients.Interfaces;
using Application.Features.Clients.DTOs;
using MediatR;

namespace Application.Features.Clients.Queries.GetClientByEmail
{
    public class GetClientByEmailHandler : IRequestHandler<GetClientByEmailQuery, ClientDto?>
    {
        private readonly IClientRepository _repository;
        private readonly IMapper _mapper;

        public GetClientByEmailHandler(IClientRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ClientDto?> Handle(GetClientByEmailQuery request, CancellationToken cancellationToken)
        {
            // Appel de la méthode spécifique de ton interface
            var client = await _repository.GetByEmailAsync(request.Email);
            
            if (client == null) return null;

            return _mapper.Map<ClientDto>(client);
        }
    }
}