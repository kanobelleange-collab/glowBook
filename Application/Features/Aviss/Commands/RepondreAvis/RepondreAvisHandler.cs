using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Application.Features.Aviss.Interfaces;
using Domain.Entities;
using Application.Features.Aviss.Commands.RepondreAvis;

namespace Application.Features.Aviss.Commands.RepondreAvis.RepondreAvisHandler
{
public class RepondreAvisHandler : IRequestHandler<RepondreAvisCommand, bool>
{
    private readonly IAvisRepository _repository;
    public RepondreAvisHandler(IAvisRepository repository) => _repository = repository;

    public async Task<bool> Handle(RepondreAvisCommand request, CancellationToken cancellationToken)
    {
        var avis = await _repository.GetByIdAsync(request.AvisId);
        if (avis == null) return false;

        avis.Repondre(request.Reponse);
        await _repository.UpdateAsync(avis);
        return true;
    }
}
}