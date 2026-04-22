using Application.Features.Admin.Interfaces;
using Dapper;
using Domain.Entities;
using System.Data;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Repository pour les opérations admin, utilisant Dapper pour économiser la RAM comme un serveur de jeu optimisé.
    /// </summary>
    public class AdminRepository : IAdminRepository
    {
        private readonly IDbConnection _db;

        public AdminRepository(IDbConnection db) => _db = db;

        public async Task<List<Etablissement>> GetPendingEtablissementsAsync()
        {
            const string sql = "SELECT * FROM Etablissements WHERE EstApprouve = 0";
            var etablissements = await _db.QueryAsync<Etablissement>(sql);
            return etablissements.ToList();
        }

        public async Task<bool> ApproveEtablissementAsync(Guid id)
        {
            const string sql = "UPDATE Etablissements SET EstApprouve = 1 WHERE Id = @Id";
            var rowsAffected = await _db.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<List<UserAccount>> GetAllUsersAsync()
        {
            const string sql = "SELECT * FROM UserAccounts";
            var users = await _db.QueryAsync<UserAccount>(sql);
            return users.ToList();
        }

        public async Task<bool> ToggleUserStatusAsync(Guid userId, bool isActive)
        {
            // Supposons qu'il y a une colonne IsActive dans UserAccounts, sinon on peut ajouter une logique
            // Pour simplifier, on peut utiliser une colonne IsBanned ou quelque chose, mais l'utilisateur n'a pas spécifié.
            // Je vais assumer qu'on met à jour une colonne IsActive dans UserAccounts.
            const string sql = "UPDATE UserAccounts SET IsActive = @IsActive WHERE Id = @Id";
            var rowsAffected = await _db.ExecuteAsync(sql, new { Id = userId, IsActive = isActive });
            return rowsAffected > 0;
        }

        public async Task<AdminStatsDto> GetGlobalStatsAsync()
        {
            // Utilise des requêtes séparées pour compter, comme compter les joueurs en ligne
            var totalClients = await _db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Clients");
            var totalSalons = await _db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Etablissements");
            var totalRDV = await _db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM RendezVous");

            return new AdminStatsDto(totalClients, totalSalons, totalRDV);
        }
    }
}