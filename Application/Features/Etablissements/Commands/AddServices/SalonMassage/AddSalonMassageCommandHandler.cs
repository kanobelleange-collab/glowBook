using MediatR;
using Domain.Entities;
using Application.Features.Etablissements.Interfaces;

namespace Application.Features.Etablissements.Commands.AddServices.SalonMassage
{
    public class AddSalonMassageHandler : IRequestHandler<AddSalonMassageCommand, Guid>
    {
        private readonly IEtablissementRepository _repository;

        public AddSalonMassageHandler(IEtablissementRepository repository)
            => _repository = repository;

        public async Task<Guid> Handle(AddSalonMassageCommand request, CancellationToken cancellationToken)
        {
            var etablissement = await _repository.GetByIdAsync(request.EtablissementId)
                ?? throw new Exception("Établissement introuvable.");

            var service = new EtablissementService(etablissement.Id, "SalonMassage");

            foreach (var p in request.Prestations)
                service.AjouterPrestation(new Prestation(
                    service.Id, p.Nom, p.Prix, p.DureeMinutes, p.Description));

            etablissement.AjouterService(service);
            await _repository.UpdateAsync(etablissement);

            return service.Id;
        }
    }
}