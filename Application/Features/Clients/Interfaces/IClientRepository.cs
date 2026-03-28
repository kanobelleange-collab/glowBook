using System;
using Domain.Entities;

namespace Application.Features.Clients.Interfaces
{
    public interface IClientRepository
    {
        // Récupérer un client par son Id
        Task<Client?> GetByIdAsync(Guid id);

        // Récupérer un client par son email (pour la connexion)
        Task<Client?> GetByEmailAsync(string email);

        // Vérifier si un email est déjà utilisé
        Task<bool> EmailExisteAsync(string email);

        // Ajouter un nouveau client
        Task AddAsync(Client client);

        // Mettre à jour un client existant
        Task MettreAJourAsync(Client client);

        Task<List<Client>> GetAllAsync();

        // Supprimer un client
        Task SupprimerAsync(Guid id);
    }
}