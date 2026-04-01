using MediatR;
using Application.Features.Payements.Interfaces;
using Domain.Entities;
using Application.Features.Payements.Commands.EchouerPaiement;
namespace Application.Features.Payements.Commands.AnnulerPaiement
{
    public class AnnulerPaiementHandler : IRequestHandler<AnnulerPaiementCommand, bool>
    {
        private readonly IPaiementRepository _repository;

        public AnnulerPaiementHandler(IPaiementRepository repository) => _repository = repository;

        public async Task<bool> Handle(AnnulerPaiementCommand request, CancellationToken cancellationToken)
        {
            var paiement = await _repository.GetByIdAsync(request.PaiementId);
            if (paiement == null) return false;

            // Utilisation de la méthode métier de l'entité
            paiement.Echouer();

            // Persistance via UpdateAsync
            await _repository.UpdateAsync(paiement);
            return true;
        }
    }
}