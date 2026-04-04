
using Domain.Entities;

namespace Application.Features.Prestations.Interfaces
{
    public interface IPrestationRepository
    {
        //  Récupérer une prestation par son Id
        Task<Prestation?> GetByIdAsync(Guid id);

        //  Récupérer toutes les prestations d'un service
        Task<List<Prestation>> GetByServiceAsync(Guid serviceId);

        // Récupérer toutes les prestations d'un établissement
        Task<List<Prestation>> GetByEtablissementAsync(Guid etablissementId);

      


        //  Rechercher par nom
        Task<List<Prestation>> RechercherAsync(string motCle);

        // Ajouter une prestation
        Task AddAsync(Prestation prestation);

        //  Mettre à jour une prestation
        Task UpdateAsync(Prestation prestation);

        //  Supprimer une prestation
        Task DeleteAsync(Guid id);
    }
}