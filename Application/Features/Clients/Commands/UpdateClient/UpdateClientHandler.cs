using AutoMapper;
using Domain.Entities;
using Application.Features.Clients.Interfaces;
using Application.Features.Clients.DTOs;
using MediatR;
using Application.Features.Clients.Commands.UpdateClient;

namespace Application.Features.Clients.Commands.CreateClient;
public class UpdateClientHandler : IRequestHandler<UpdateClientCommand, ClientDto?>
{
    private readonly IClientRepository _repository;
    private readonly IMapper _mapper;

    public UpdateClientHandler(IClientRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ClientDto?> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        var client= _mapper.Map<Client>(request);
    if (client==null) return null;
        await _repository.UpdateAsync(client);
        return _mapper.Map<ClientDto>(client);
       

    }
}
