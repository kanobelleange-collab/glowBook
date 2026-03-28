using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Domain.Entities;
using Infrastructure.DBcontext;
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

        /// <summary>
        /// Récupère un paiement par son Id
        /// </summary>
        public async Task<Paiement?> GetByIdAsync(Guid id)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = @"
                    SELECT * FROM Paiements 
                    WHERE Id = @Id";

                return await connection.QueryFirstOrDefaultAsync<Paiement>(
                    query,
                    new { Id = id.ToString() });
            }
        }

        /// <summary>
        /// Récupère un paiement par l'Id du rendez-vous
        /// </summary>
        public async Task<Paiement?> GetByRendezVousIdAsync(Guid rendezVousId)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = @"
                    SELECT * FROM Paiements 
                    WHERE RendezVousId = @RendezVousId
                    ORDER BY DateCreation DESC
                    LIMIT 1";

                return await connection.QueryFirstOrDefaultAsync<Paiement>(
                    query,
                    new { RendezVousId = rendezVousId.ToString() });
            }
        }

        /// <summary>
        /// Récupère un paiement par l'Id de transaction CinetPay
        /// </summary>
        public async Task<Paiement?> GetByTransactionIdAsync(string transactionId)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = @"
                    SELECT * FROM Paiements 
                    WHERE TransactionId = @TransactionId";

                return await connection.QueryFirstOrDefaultAsync<Paiement>(
                    query,
                    new { TransactionId = transactionId });
            }
        }

        /// <summary>
        /// Récupère tous les paiements d'un client
        /// </summary>
        public async Task<List<Paiement>> GetByClientAsync(Guid clientId)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = @"
                    SELECT p.* FROM Paiements p
                    INNER JOIN RendezVous rv ON p.RendezVousId = rv.Id
                    WHERE rv.ClientId = @ClientId
                    ORDER BY p.DateCreation DESC";

                var result = await connection.QueryAsync<Paiement>(
                    query,
                    new { ClientId = clientId.ToString() });

                return result.ToList();
            }
        }

        /// <summary>
        /// Récupère les paiements par statut
        /// </summary>
        public async Task<List<Paiement>> GetByStatutAsync(StatutPaiement statut)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = @"
                    SELECT * FROM Paiements 
                    WHERE Statut = @Statut
                    ORDER BY DateCreation DESC";

                var result = await connection.QueryAsync<Paiement>(
                    query,
                    new { Statut = statut.ToString() });

                return result.ToList();
            }
        }

        /// <summary>
        /// Ajoute un nouveau paiement
        /// </summary>
        public async Task AjouterAsync(Paiement paiement)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = @"
                    INSERT INTO Paiements 
                    (Id, RendezVousId, ClientId, Montant, Statut, MethodePaiement, 
                     TransactionId, DateCreation, DateModification)
                    VALUES 
                    (@Id, @RendezVousId, @ClientId, @Montant, @Statut, @MethodePaiement, 
                     @TransactionId, @DateCreation, @DateModification)";

                await connection.ExecuteAsync(query, new
                {
                    Id = paiement.Id.ToString(),
                    RendezVousId = paiement.RendezVousId.ToString(),
                    ClientId = paiement.ClientId.ToString(),
                    paiement.Montant,
                    Statut = paiement.Statut.ToString(),
                    MethodePaiement = paiement.MethodePaiement,
                    paiement.TransactionId,
                    DateCreation = DateTime.UtcNow,
                    DateModification = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Met à jour un paiement existant
        /// </summary>
        public async Task MettreAJourAsync(Paiement paiement)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = @"
                    UPDATE Paiements 
                    SET RendezVousId = @RendezVousId,
                        ClientId = @ClientId,
                        Montant = @Montant,
                        Statut = @Statut,
                        MethodePaiement = @MethodePaiement,
                        TransactionId = @TransactionId,
                        DateModification = @DateModification
                    WHERE Id = @Id";

                var rowsAffected = await connection.ExecuteAsync(query, new
                {
                    RendezVousId = paiement.RendezVousId.ToString(),
                    ClientId = paiement.ClientId.ToString(),
                    paiement.Montant,
                    Statut = paiement.Statut.ToString(),
                    MethodePaiement = paiement.MethodePaiement,
                    paiement.TransactionId,
                    DateModification = DateTime.UtcNow,
                    Id = paiement.Id.ToString()
                });

                if (rowsAffected == 0)
                    throw new InvalidOperationException($"Paiement avec l'Id {paiement.Id} non trouvé");
            }
        }
    }
}