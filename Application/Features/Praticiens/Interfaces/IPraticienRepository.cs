using System;
using Application.Features.Praticiens.Interfaces;
using Domain.Entities;

namespace Application.Features.Praticiens.Interfaces
{

    public interface IPraticienRepository
    {
        // Récupérer un praticien par son Id
        Task<Praticien?> GetByIdAsync(Guid id);

        // Récupérer tous les praticiens d'un établissement
        Task<List<Praticien>> GetByEtablissementAsync(Guid etablissementId);

        // Récupérer les praticiens disponibles à une date/heure précise
        Task<List<Praticien>> GetDisponiblesAsync(Guid etablissementId, DateTime dateHeure);

        // Récupérer par spécialité
        Task<List<Praticien>> GetBySpecialiteAsync(string specialite);

        // Ajouter un praticien
        Task AjouterAsync(Praticien praticien);

        // Mettre à jour un praticien
        Task MettreAJourAsync(Praticien praticien);

        // Supprimer un praticien
        Task SupprimerAsync(Guid id);
    }
}