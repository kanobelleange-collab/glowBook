using System;
using Domain.Entities;
using Application.Features.Clients.DTOs;

namespace Application.Features.Clients.Interfaces
{
    public interface IClientRepository
    {
        // Récupérer un client par son Id
        Task<ClientDto?> GetByIdAsync(Guid id);

        // Ajouter un nouveau client
        Task<Client> AddAsync(Client client);

        // Mettre à jour un client existant
        Task<ClientDto> UpdateAsync(Client client);

        Task<List<ClientDto>> GetAllAsync();

        // Supprimer un client
        Task<Guid> DeleteAsync(Guid id);
    }
}