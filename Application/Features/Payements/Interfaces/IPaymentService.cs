using System;
using Domain.Entities;

namespace Application.Features.Payements.Interfaces
{

    // Interface pour le paiement en ligne
   

    public interface IPaymentService
    {
        // Créer une session de paiement et retourner le lien
        Task<string> CreerSessionPaiementAsync(
            Guid rendezVousId,
            decimal montant,
            string methodePaiement,
            string urlRetourSucces,
            string urlRetourEchec);

        // Vérifier le statut d'un paiement via son ID transaction
        Task<bool> VerifierPaiementAsync(string transactionId);

        // Rembourser un paiement
        Task<bool> RembourserAsync(string transactionId, decimal montant);
    }
}