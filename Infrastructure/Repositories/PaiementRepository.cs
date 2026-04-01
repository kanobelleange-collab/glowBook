using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Domain.Entities;
using Application.Features.Payements.Interfaces;
using Domain.Enum;
using Application.Common.Interfaces;

namespace Infrastructure.Repositories
{
    public class PaiementRepository : IPaiementRepository
    {
        private readonly IApplicationDbContext _dbContext;

        public PaiementRepository(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Paiement?> GetByIdAsync(Guid id)
        {
            using var connection = _dbContext.CreateConnection();
            const string query = "SELECT * FROM Paiements WHERE Id = @Id";
            
            return await connection.QueryFirstOrDefaultAsync<Paiement>(query, new { Id = id.ToString() });
        }

        public async Task<Paiement?> GetByRendezVousIdAsync(Guid rendezVousId)
        {
            using var connection = _dbContext.CreateConnection();
            const string query = @"
                SELECT * FROM Paiements 
                WHERE RendezVousId = @RendezVousId
                ORDER BY DateCreation DESC
                LIMIT 1";

            return await connection.QueryFirstOrDefaultAsync<Paiement>(query, new { RendezVousId = rendezVousId.ToString() });
        }

        public async Task<Paiement?> GetByTransactionIdAsync(string transactionId)
        {
            using var connection = _dbContext.CreateConnection();
            const string query = "SELECT * FROM Paiements WHERE TransactionId = @TransactionId";

            return await connection.QueryFirstOrDefaultAsync<Paiement>(query, new { TransactionId = transactionId });
        }

        public async Task<List<Paiement>> GetByClientAsync(Guid clientId)
        {
            using var connection = _dbContext.CreateConnection();
            // Note : J'utilise le ClientId directement présent dans ton entité Paiement 
            // pour éviter une jointure inutile avec RendezVous
            const string query = @"
                SELECT * FROM Paiements 
                WHERE ClientId = @ClientId
                ORDER BY DateCreation DESC";

            var result = await connection.QueryAsync<Paiement>(query, new { ClientId = clientId.ToString() });
            return result.ToList();
        }

        public async Task<List<Paiement>> GetByStatutAsync(StatutPaiement statut)
        {
            using var connection = _dbContext.CreateConnection();
            const string query = "SELECT * FROM Paiements WHERE Statut = @Statut ORDER BY DateCreation DESC";

            // On passe le nom de l'enum pour le stockage en string
            var result = await connection.QueryAsync<Paiement>(query, new { Statut = statut.ToString() });
            return result.ToList();
        }

        public async Task AddAsync(Paiement paiement)
        {
            using var connection = _dbContext.CreateConnection();
            const string query = @"
                INSERT INTO Paiements 
                (Id, RendezVousId, ClientId, Montant, Devise, Statut, MethodePaiement, 
                 TransactionId, LienPaiement, DateCreation)
                VALUES 
                (@Id, @RendezVousId, @ClientId, @Montant, @Devise, @Statut, @MethodePaiement, 
                 @TransactionId, @LienPaiement, @DateCreation)";

            await connection.ExecuteAsync(query, new
            {
                Id = paiement.Id.ToString(),
                RendezVousId = paiement.RendezVousId.ToString(),
                ClientId = paiement.ClientId.ToString(),
                paiement.Montant,
                paiement.Devise,
                Statut = paiement.Statut.ToString(),
                paiement.MethodePaiement,
                paiement.TransactionId,
                paiement.LienPaiement,
                paiement.DateCreation
            });
        }

        public async Task UpdateAsync(Paiement paiement)
        {
            using var connection = _dbContext.CreateConnection();
            const string query = @"
                UPDATE Paiements 
                SET Statut = @Statut,
                    TransactionId = @TransactionId,
                    LienPaiement = @LienPaiement,
                    DateConfirmation = @DateConfirmation
                WHERE Id = @Id";

            var rowsAffected = await connection.ExecuteAsync(query, new
            {
                Statut = paiement.Statut.ToString(),
                paiement.TransactionId,
                paiement.LienPaiement,
                paiement.DateConfirmation,
                Id = paiement.Id.ToString()
            });

            if (rowsAffected == 0)
                throw new Exception($"Échec de la mise à jour : Paiement {paiement.Id} introuvable.");
        }
    }
}