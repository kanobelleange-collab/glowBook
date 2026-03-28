using System;
using System.Threading;
using MediatR;
using AutoMapper;
using Domain.Entities;
using Application.Features.Clients.DTOs;
using Application.Features.Clients.Interfaces;
using Application.Features.Clients.Queries.GetById;


namespace Application.Features.Clients.Queries.GetById
{
    public class GetByIdClientHandler : IRequestHandler<GetByIdClientQuery, ClientDto>
    {
        private readonly IClientRepository _repository;
        private readonly IMapper _mapper;

        public GetByIdClientHandler(IClientRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ClientDto> Handle(GetByIdClientQuery request, CancellationToken cancellationToken)
        {
            var client = await _repository.GetByIdAsync(request.Id);
            if (client == null) throw new KeyNotFoundException($"Client avec l'ID {request.Id} non trouvé.");

            return _mapper.Map<ClientDto>(client);
        }
    }
}