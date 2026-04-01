using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Application.Features.Clients.Commands.DeleteClient;
using Application.Features.Clients.Interfaces;

namespace Application.Features.Clients.Commands.DeleteClient.DeleteClientHandler;

    public class DeleteClientHandler : IRequestHandler<DeleteClientCommand, Guid>
{
    private readonly IClientRepository _repository;

    public DeleteClientHandler(IClientRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
    {
        // Note : Pense à modifier ton interface IClientRepository pour accepter un int au lieu d'un Guid
         var client=await _repository.DeleteAsync(request.Id);
         return client;
    }
}