using MediatR;
using Domain.Entities;
using Application.Features.Etablissements.Interfaces;

namespace Application.Features.Etablissements.Commands.AddServices.SpaBeaute
{
    public class AddSpaBeauteHandler : IRequestHandler<AddSpaBeauteCommand, Guid>
    {
        private readonly IEtablissementRepository _repository;

        public AddSpaBeauteHandler(IEtablissementRepository repository)
            => _repository = repository;

        public async Task<Guid> Handle(AddSpaBeauteCommand request, CancellationToken cancellationToken)
        {
            var etablissement = await _repository.GetByIdAsync(request.EtablissementId)
                ?? throw new Exception("Établissement introuvable.");

            var service = new EtablissementService(etablissement.Id, "SpaBeaute");

            foreach (var p in request.Prestations)
                service.AjouterPrestation(new Prestation(
                    service.Id, p.Nom, p.Prix, p.DureeMinutes, p.Description));

            etablissement.AjouterService(service);
            await _repository.UpdateAsync(etablissement);

            return service.Id;
        }
    }
}