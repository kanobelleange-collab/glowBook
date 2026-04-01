using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Application.Features.Aviss.Interfaces;
using Domain.Entities;
using Application.Features.Aviss.Commands.CreerAvis;

namespace Application.Features.Aviss.Commands.CreerAvis.CreerAvisHandler
{

public class CreerAvisHandler : IRequestHandler<CreerAvisCommand, Guid>
{
    private readonly IAvisRepository _repository;

    public CreerAvisHandler(IAvisRepository repository) => _repository = repository;

    public async Task<Guid> Handle(CreerAvisCommand request, CancellationToken cancellationToken)
    {
        // 1. Sécurité : Un seul avis par rendez-vous
        if (await _repository.AvisDejaExisteAsync(request.RendezVousId))
            throw new Exception("Un avis a déjà été laissé pour ce rendez-vous.");

        // 2. Création de l'entité (Validation de la note incluse dans le constructeur)
        var avis = new Avis(request.ClientId, request.EtablissementId, request.RendezVousId, request.Note, request.Commentaire);

        await _repository.AddAsync(avis);
        return avis.Id;
    }
}
}