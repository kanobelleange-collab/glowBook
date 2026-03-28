using System;
using Domain.Entities;


namespace Application.Features.Rendevou.Interfaces
{


    public interface IRendezVousRepository
    {
        // Récupérer un rendez-vous par son Id
        Task<RendezVous?> GetByIdAsync(Guid id);

        // Récupérer tous les RDV d'un client
        Task<List<RendezVous>> GetByClientAsync(Guid clientId);

        // Récupérer tous les RDV d'un praticien
        Task<List<RendezVous>> GetByPraticienAsync(Guid praticienId);

        // Récupérer tous les RDV d'un établissement
        Task<List<RendezVous>> GetByEtablissementAsync(Guid etablissementId);

        // Récupérer les RDV d'un praticien pour une journée précise
        Task<List<RendezVous>> GetByPraticienEtDateAsync(Guid praticienId, DateTime date);

        // Vérifier si un praticien est déjà occupé à un créneau donné
        Task<bool> CreneauDejaOccupeAsync(Guid praticienId, DateTime dateHeure);

        // Récupérer les RDV par statut
        Task<List<RendezVous>> GetByStatutAsync(StatutRendezVous statut);

        // Ajouter un rendez-vous
        Task AddAsync(RendezVous rendezVous);

        // Mettre à jour un rendez-vous (statut, annulation...)
        Task MettreAJourAsync(RendezVous rendezVous);
    }
}