using System;
using Domain.Entities;

namespace Application.Features.ServiceEsthtiques.Interfaces
{

    public interface IServiceEsthetiqueRepository
    {
        // Récupérer un service par son Id
        Task<ServiceEsthetique?> GetByIdAsync(Guid id);

        // Récupérer tous les services d'un établissement
        Task<List<ServiceEsthetique>> GetByEtablissementAsync(Guid etablissementId);

        // Récupérer les services par catégorie
        Task<List<ServiceEsthetique>> GetByCategorieAsync(string categorie);

        // Récupérer les services disponibles uniquement
        Task<List<ServiceEsthetique>> GetDisponiblesAsync(Guid etablissementId);

        // Ajouter un service
        Task AjouterAsync(ServiceEsthetique service);

        // Mettre à jour un service
        Task MettreAJourAsync(ServiceEsthetique service);

        // Supprimer un service
        Task SupprimerAsync(Guid id);
    }
}