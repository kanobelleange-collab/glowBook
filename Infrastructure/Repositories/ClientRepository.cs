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

        // 3. Récupérer un client par son email
        public async Task<ClientDto?> GetByEmailAsync(string email)
        {
            using var connection = _context.CreateConnection();
            const string sql = "SELECT * FROM Clients WHERE Email = @Email";
            return await connection.QueryFirstOrDefaultAsync<ClientDto>(sql, new { Email = email });
        }

        // 4. Ajouter un nouveau client
        public async Task<Client?> AddAsync(Client client)
        {
            using var connection = _context.CreateConnection();
            const string sql = @"
                INSERT INTO Clients (Nom, Email, Telephone, Quartier, Ville, DateInscription, EstActif)
                VALUES (@Nom, @Email, @Telephone, @Quartier, @Ville, @DateInscription, @EstActif);
                SELECT LAST_INSERT_ID();";

            // On récupère l'ID généré par MySQL pour mettre à jour l'objet client en mémoire
            var clients = await connection.ExecuteScalarAsync<Client?>(sql, client);
            return clients;
            
            // Note : Comme ton Id est 'private set', assure-toi que Dapper peut y accéder 
            // ou utilise une méthode de l'entité pour le définir si nécessaire.
        }

        // 5. Mettre à jour un client
        public async Task<ClientDto?> UpdateAsync(Client client)
        {
            using var connection = _context.CreateConnection();
            const string sql = @"
                UPDATE Clients 
                SET Nom = @Nom, 
                    Email = @Email, 
                    Telephone = @Telephone, 
                    Quartier = @Quartier, 
                    Ville = @Ville, 
                    EstActif = @EstActif 
                WHERE Id = @Id";

            return await connection.QueryFirstOrDefaultAsync<ClientDto?>(sql, client);
            
        }

        // 6. Supprimer un client (ID en int pour la cohérence)
        public async Task<Guid> DeleteAsync(Guid id) 
        {
            using var connection = _context.CreateConnection();
            const string sql = "DELETE FROM Clients WHERE Id = @Id";
            var client=await connection.QueryFirstOrDefaultAsync<Guid>(sql, new { Id = id });
            return client;
        }

    }
}