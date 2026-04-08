using MediatR;
using Domain.Entities;
using Application.Features.Etablissements.Interfaces;

namespace Application.Features.Etablissements.Commands.AddServices.SalonCoiffure
{
    public class AddSalonCoiffureHandler : IRequestHandler<AddSalonCoiffureCommand, Guid>
    {
        private readonly IEtablissementRepository _repository;

        public AddSalonCoiffureHandler(IEtablissementRepository repository)
            => _repository = repository;

        public async Task<Guid> Handle(AddSalonCoiffureCommand request, CancellationToken cancellationToken)
{
    // 1. Vérifier si l'établissement existe
    var etablissement = await _repository.GetByIdAsync(request.EtablissementId)
        ?? throw new Exception("Établissement introuvable.");

    // 2. Créer l'objet service
    var service = new EtablissementService(etablissement.Id, "SalonCoiffure");

    // --- CE QU'IL FAUT AJOUTER : ---
    // On crée un objet "anonyme" avec les champs spécifiques de la commande
    var specificData = new 
    {
        request.TypeServiceNom,
        request.SpecialitesTresse,
        request.TypesCheveux,
        request.AccepteHommes,
        request.AccepteEnfants
    };

    // On transforme cet objet en texte JSON pour la colonne 'Data' de la base
    service.Data = System.Text.Json.JsonSerializer.Serialize(specificData);
    // -------------------------------

    // 3. Enregistrer
    await _repository.AddServiceAsync(service); 
    await _repository.UpdateAsync(etablissement);

    return service.Id;
}
}
}