using MediatR;
using Domain.Entities;
using Application.Features.Etablissements.Interfaces;

namespace Application.Features.Etablissements.Commands.AddServices.CabinetProthetiste
{
    public class AddCabinetProthetisteHandler : IRequestHandler<AddCabinetProthetisteCommand, Guid>
    {
        private readonly IEtablissementRepository _repository;

        public AddCabinetProthetisteHandler(IEtablissementRepository repository)
            => _repository = repository;

        public async Task<Guid> Handle(AddCabinetProthetisteCommand request, CancellationToken cancellationToken)
        {
            var etablissement = await _repository.GetByIdAsync(request.EtablissementId)
                ?? throw new Exception("Établissement introuvable.");

            var service = new EtablissementService(etablissement.Id, "CabinetProthetiste");
             var specificData = new 
    {
        request.ProposeProtheseOngles,
        request.ProposeExtensionCils,
        request.ProposeProtheseCapillaire,
        request.MatieresOngles,
        request.StylesCils
    };

    // On transforme cet objet en texte JSON pour la colonne 'Data' de la base
    service.Data = System.Text.Json.JsonSerializer.Serialize(specificData);

            // foreach (var p in request.Prestations)
            //     service.AjouterPrestation(new Prestation(
            //         service.Id, p.Nom, p.Prix, p.DureeMinutes, p.Description));

             await _repository.AddServiceAsync(service); 
             await _repository.UpdateAsync(etablissement);

            return service.Id;
        }
    }
}