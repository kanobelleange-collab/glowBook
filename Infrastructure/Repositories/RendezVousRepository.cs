using Application.Features.Rendevou.Interfaces;
using Domain.Entities;
using Infrastructure.DBcontext;
using Dapper;
using System.Data;

namespace Infrastructure.Repositories
{
    public class RendezVousRepository : IRendezVousRepository
    {
        private readonly ApplicationDbContext _context;

        public RendezVousRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Récupérer un RDV par son Id
        public async Task<RendezVous?> GetByIdAsync(Guid id)
        {
            using var connection = _context.CreateConnection();
            const string sql = "SELECT * FROM RendezVous WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<RendezVous>(sql, new { Id = id });
        }

        // 2. Historique Client
        public async Task<List<RendezVous>> GetByClientAsync(Guid clientId)
        {
            using var connection = _context.CreateConnection();
            const string sql = "SELECT * FROM RendezVous WHERE ClientId = @ClientId ORDER BY DateHeure DESC";
            var result = await connection.QueryAsync<RendezVous>(sql, new { ClientId = clientId });
            return result.ToList();
        }

        // 3. Planning Praticien
        public async Task<List<RendezVous>> GetByEmployeeAsync(Guid EmployeeId)
        {
            using var connection = _context.CreateConnection();
            const string sql = "SELECT * FROM RendezVous WHERE PraticienId = @EmployeeId ORDER BY DateHeure ASC";
            var result = await connection.QueryAsync<RendezVous>(sql, new { EmployeeId = EmployeeId });
            return result.ToList();
        }

        // 4. RDV par Établissement
        public async Task<List<RendezVous>> GetByEtablissementAsync(Guid etablissementId)
        {
            using var connection = _context.CreateConnection();
            const string sql = "SELECT * FROM RendezVous WHERE EtablissementId = @EtablissementId ORDER BY DateHeure DESC";
            var result = await connection.QueryAsync<RendezVous>(sql, new { EtablissementId = etablissementId });
            return result.ToList();
        }

        // 5. Agenda du jour pour un praticien
        public async Task<List<RendezVous>> GetByEmployeeEtDateAsync(Guid EmployeeId, DateTime dateHeure)
        {
            using var connection = _context.CreateConnection();
            // Utilisation de DATE() pour comparer uniquement le jour sans l'heure
            const string sql = @"
                SELECT * FROM RendezVous 
                WHERE EmployeeId = @EmployeeId 
                AND DATE(DateHeure) = DATE(@Date)
                AND Statut != @StatutAnnule";
            
            var result = await connection.QueryAsync<RendezVous>(sql, new { 
                EmployeeId = EmployeeId, 
                Date = dateHeure,
                StatutAnnule = (int)StatutRendezVous.Annule 
            });
            return result.ToList();
        }

        // 6. Vérification anti-doublon (Crucial pour la création)
        public async Task<bool> CreneauDejaOccupeAsync(Guid EmployeeId, DateTime dateHeure)
        {
            using var connection = _context.CreateConnection();
            const string sql = @"
                SELECT COUNT(1) FROM RendezVous 
                WHERE EmployeeId = @EmployeeId 
                AND DateHeure = @DateHeure 
                AND Statut IN (@Attente, @Confirme)";
            
            var count = await connection.ExecuteScalarAsync<int>(sql, new { 
                EmployeeId = EmployeeId, 
                DateHeure = dateHeure,
                Attente = (int)StatutRendezVous.EnAttente,
                Confirme = (int)StatutRendezVous.Confirme
            });
            
            return count > 0;
        }

        // 7. Filtrage par Statut
        public async Task<List<RendezVous>> GetByStatutAsync(StatutRendezVous statut)
        {
            using var connection = _context.CreateConnection();
            const string sql = "SELECT * FROM RendezVous WHERE Statut = @Statut";
            var result = await connection.QueryAsync<RendezVous>(sql, new { Statut = (int)statut });
            return result.ToList();
        }

        // 8. Insertion (CREATE)
        public async Task AddAsync(RendezVous rdv)
        {
            using var connection = _context.CreateConnection();
            const string sql = @"
                INSERT INTO RendezVous (  ClientId, EmployeeId, ServiceId, EtablissementId,DateHeure, Statut, Prix, NotesClient, RaisonAnnulation)
                VALUES (  @ClientId, @EmployeeId, @ServiceId, @EtablissementId,@DateHeure, @Statut, @Prix, @NotesClient, @RaisonAnnulation)";
            
            await connection.ExecuteAsync(sql, rdv);
        }

        // 9. Mise à jour (UPDATE)
        public async Task MettreAJourAsync(RendezVous rdv)
        {
            using var connection = _context.CreateConnection();
            const string sql = @"
                UPDATE RendezVous 
                SET Statut = @Statut, 
                    RaisonAnnulation = @RaisonAnnulation,
                WHERE Id = @Id";
            
            await connection.ExecuteAsync(sql, rdv);
        }
    }
}