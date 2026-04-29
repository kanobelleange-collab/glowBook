using Application.Features.Clients.Interfaces;
using Domain.Entities;
using Application.Features.Clients.DTOs;
using Infrastructure.DBcontext;
using Dapper;
using System.Data;

namespace Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly ApplicationDbContext _context;

        public ClientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Récupérer tous les clients
        public async Task<List<ClientDto>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            const string sql = "SELECT * FROM Clients";
            var clients = await connection.QueryAsync<ClientDto>(sql);
            return clients.ToList();
        }

        // 2. Récupérer un client par son ID
        public async Task<ClientDto?> GetByIdAsync(Guid id)
        {
            using var connection = _context.CreateConnection();
            const string sql = "SELECT * FROM Clients WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<ClientDto>(sql, new { Id = id });
        }

        // 4. Ajouter un nouveau client
        public async Task<Client> AddAsync(Client client)
        {
            using var connection = _context.CreateConnection();

            // Correction 1 : On ajoute explicitement 'Id' dans les colonnes et les valeurs
            const string sql = @"
        INSERT INTO Clients (Id, Nom, Email, Telephone, Quartier, Ville, DateInscription, EstActif)
        VALUES (@Id, @Nom, @Email, @Telephone, @Quartier, @Ville, @DateInscription, @EstActif)";

            // Correction 2 : On utilise ExecuteAsync (Scalar n'est pas nécessaire pour un Guid généré en C#)
            await connection.ExecuteAsync(sql, client);

            return client;
        }


        // 5. Mettre à jour un client
        public async Task<ClientDto> UpdateAsync(Client client)
        {
            using var connection = _context.CreateConnection();
            const string sql = @"
        UPDATE Clients 
        SET Nom = @Nom, 
            Prenom = @Prenom,
            Email = @Email, 
            Telephone = @Telephone, 
            Quartier = @Quartier, 
            Ville = @Ville, 
            EstActif = @EstActif 
        WHERE Id = @Id"; // <-- Pas de virgule avant le WHERE !

            await connection.ExecuteAsync(sql, client);

            return new ClientDto
            {
                Nom = client.Nom,
                Telephone = client.Telephone,
                Quartier = client.Quartier,
                Ville = client.Ville,
                EstActif = client.EstActif
            };
        }

        // 6. Supprimer un client (ID en int pour la cohérence)
        public async Task<Guid> DeleteAsync(Guid id)
        {
            using var connection = _context.CreateConnection();
            const string sql = "DELETE FROM Clients WHERE Id = @Id";
            var client = await connection.QueryFirstOrDefaultAsync<Guid>(sql, new { Id = id });
            return client;
        }

    }
}