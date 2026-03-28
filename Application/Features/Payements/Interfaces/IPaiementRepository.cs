using Domain.Entities;
using Domain.Enum;


namespace Application.Features.Payements.Interfaces
{
    public interface IPaiementRepository
    {
        // Récupérer un paiement par son Id
        Task<Paiement?> GetByIdAsync(Guid id);

        // Récupérer un paiement par l'Id du rendez-vous
        Task<Paiement?> GetByRendezVousIdAsync(Guid rendezVousId);

        // Récupérer un paiement par l'Id de transaction CinetPay
        Task<Paiement?> GetByTransactionIdAsync(string transactionId);

        // Récupérer tous les paiements d'un client
        Task<List<Paiement>> GetByClientAsync(Guid clientId);

        // Récupérer les paiements par statut
        Task<List<Paiement>> GetByStatutAsync(StatutPaiement statut);

        // Ajouter un nouveau paiement
        Task AjouterAsync(Paiement paiement);

        // Mettre à jour un paiement existant
        Task MettreAJourAsync(Paiement paiement);
        
    }
}