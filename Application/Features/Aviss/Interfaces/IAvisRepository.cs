using System;
using Domain.Entities;

namespace Application.Features.Aviss.Interfaces
{
    public interface IAvisRepository
    {
        // Récupérer un avis par son Id
        Task<Avis?> GetByIdAsync(Guid id);

        // Récupérer tous les avis d'un établissement
        Task<List<Avis>> GetByEtablissementAsync(Guid etablissementId);

        // Récupérer tous les avis d'un client
        Task<List<Avis>> GetByClientAsync(Guid clientId);

        // Vérifier si le client a déjà laissé un avis pour ce RDV
        Task<bool> AvisDejaExisteAsync(Guid rendezVousId);

        // Calculer la note moyenne d'un établissement
        Task<double> CalculerNoteMoyenneAsync(Guid etablissementId);

        // Ajouter un avis
        Task AjouterAsync(Avis avis);

        // Mettre à jour un avis (réponse du salon, masquer...)
        Task MettreAJourAsync(Avis avis);
    }
}