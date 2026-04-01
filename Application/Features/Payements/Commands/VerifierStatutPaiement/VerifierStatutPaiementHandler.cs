using MediatR;
using Application.Features.Payements.Interfaces;
using Domain.Entities;
using Application.Features.Payements.VerifierStatutPaiement;
namespace Application.Features.Payements.VerificationStatutPaiement.VerifierStatutPaiementHandler;
public class VerifierStatutPaiementHandler : IRequestHandler<VerifierStatutPaiementCommand, bool>
{
    private readonly IPaiementRepository _repository;
    private readonly IPaymentService _paymentService;

    public VerifierStatutPaiementHandler(IPaiementRepository repository, IPaymentService paymentService)
    {
        _repository = repository;
        _paymentService = paymentService;
    }

    public async Task<bool> Handle(VerifierStatutPaiementCommand request, CancellationToken cancellationToken)
    {
        // 1. Appeler l'API externe pour savoir si l'argent est vraiment arrivé
        bool estValide = await _paymentService.VerifierPaiementAsync(request.TransactionId);

        if (estValide)
        {
            var paiement = await _repository.GetByTransactionIdAsync(request.TransactionId);
            if (paiement != null)
            {
                paiement.Confirmer(request.TransactionId);
                await _repository.UpdateAsync(paiement);
                return true;
            }
        }
        return false;
    }
}