using MediatR;
using Application.Features.Payements.Interfaces;
using Domain.Entities;
using Application.Features.Payements.Commands.ConfirmerPaiement;
namespace Application.Features.Payements.Commands.ConfirmerPaiement.ConfirmerPaiementHandler
{
    public class ConfirmerPaiementHandler : IRequestHandler<ConfirmerPaiementCommand, bool>
    {
        private readonly IPaiementRepository _repository;

        public ConfirmerPaiementHandler(IPaiementRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(ConfirmerPaiementCommand request, CancellationToken cancellationToken)
        {
            var paiement = await _repository.GetByTransactionIdAsync(request.TransactionId);
            if (paiement == null) return false;

            // Logique métier de l'entité
            paiement.Confirmer(request.TransactionId);

            // Mise à jour via le Repository
            await _repository.UpdateAsync(paiement);
            return true;
        }
    }
}