using Application.Features.Rendevou.Interfaces;
using Domain.Entities;
using Infrastructure.DBcontext;
using Dapper;
using System.Data;
using Application.Features.Rendevou.DTOs;

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

        // 3. Planning Employee
        // ✅ Corrigé : PraticienId → EmployeeId (aligné avec l'entité Domain)
        public async Task<List<RendezVous>> GetByEmployeeAsync(Guid employeeId)
        {
            using var connection = _context.CreateConnection();
            const string sql = "SELECT * FROM RendezVous WHERE EmployeeId = @EmployeeId ORDER BY DateHeure ASC";
            var result = await connection.QueryAsync<RendezVous>(sql, new { EmployeeId = employeeId });
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

        // 5. Agenda du jour pour un employee
        // ✅ Corrigé : DATE() → CAST AS DATE (compatible MySQL et SQL Server)
        public async Task<List<RendezVous>> GetByEmployeeEtDateAsync(Guid employeeId, DateTime dateHeure)
        {
            using var connection = _context.CreateConnection();
            const string sql = @"
                SELECT * FROM RendezVous 
                WHERE EmployeeId = @EmployeeId 
                AND CAST(DateHeure AS DATE) = CAST(@Date AS DATE)
                AND Statut != @StatutAnnule";

            var result = await connection.QueryAsync<RendezVous>(sql, new
            {
                EmployeeId = employeeId,
                Date = dateHeure,
                StatutAnnule = (int)StatutRendezVous.Annule
            });
            return result.ToList();
        }

        // 6. Agenda par Établissement et Date
        // ✅ Corrigé : r.Service → r.ServiceId + JOIN Services
        // ✅ Corrigé : PraticienId → EmployeeId dans les JOINs
        // ✅ Corrigé : DateCreation ajouté dans le SELECT
        public async Task<List<RendezVousDto>> GetAgendaAsync(Guid etablissementId, DateTime date, CancellationToken cancellationToken = default)
        {
            using var connection = _context.CreateConnection();
            const string sql = @"
                SELECT r.Id,
                       r.EtablissementId,
                       e.Nom       AS EtablissementNom,
                       r.EmployeeId,
                       p.Nom       AS EmployeeNom,
                       r.ClientId,
                       c.Nom       AS ClientNom,
                       c.Email     AS ClientEmail,
                       r.DateHeure,
                       r.ServiceId,
                       s.Nom       AS ServiceNom,
                       r.Statut,
                       r.Prix,
                       r.NotesClient,
                       r.RaisonAnnulation,
                       r.DateCreation
                FROM RendezVous r
                LEFT JOIN UserAccounts  c ON r.ClientId       = c.Id
                LEFT JOIN UserAccounts  p ON r.EmployeeId     = p.Id
                LEFT JOIN Etablissements e ON r.EtablissementId = e.Id
                LEFT JOIN Services       s ON r.ServiceId      = s.Id
                WHERE r.EtablissementId = @EtablissementId
                AND CAST(r.DateHeure AS DATE) = CAST(@Date AS DATE)
                AND r.Statut != @StatutAnnule
                ORDER BY r.DateHeure ASC";

            var result = await connection.QueryAsync<RendezVousDto>(sql, new
            {
                EtablissementId = etablissementId,
                Date = date,
                StatutAnnule = (int)StatutRendezVous.Annule
            });

            return result.ToList();
        }

        // 7. Vérification anti-doublon
        // ✅ Corrigé : EmployeeId cohérent avec l'entité
        public async Task<bool> CreneauDejaOccupeAsync(Guid employeeId, DateTime dateHeure)
        {
            using var connection = _context.CreateConnection();
            const string sql = @"
                SELECT COUNT(1) FROM RendezVous 
                WHERE EmployeeId = @EmployeeId 
                AND DateHeure   = @DateHeure 
                AND Statut IN (@Attente, @Confirme)";

            var count = await connection.ExecuteScalarAsync<int>(sql, new
            {
                EmployeeId = employeeId,
                DateHeure  = dateHeure,
                Attente    = (int)StatutRendezVous.EnAttente,
                Confirme   = (int)StatutRendezVous.Confirme
            });

            return count > 0;
        }

        // 8. Filtrage par Statut
        public async Task<List<RendezVous>> GetByStatutAsync(StatutRendezVous statut)
        {
            using var connection = _context.CreateConnection();
            const string sql = "SELECT * FROM RendezVous WHERE Statut = @Statut";
            var result = await connection.QueryAsync<RendezVous>(sql, new { Statut = (int)statut });
            return result.ToList();
        }

        // 9. Insertion (CREATE)
        // ✅ Corrigé : PraticienId → EmployeeId, DateCreation ajouté
        public async Task AddAsync(RendezVous rdv)
        {
            using var connection = _context.CreateConnection();
            const string sql = @"
                INSERT INTO RendezVous 
                    (Id, ClientId, EmployeeId, ServiceId, EtablissementId, DateHeure, Statut, Prix, NotesClient, RaisonAnnulation, DateCreation)
                VALUES 
                    (@Id, @ClientId, @EmployeeId, @ServiceId, @EtablissementId, @DateHeure, @Statut, @Prix, @NotesClient, @RaisonAnnulation, @DateCreation)";

            await connection.ExecuteAsync(sql, rdv);
        }

        // 10. Mise à jour (UPDATE)
        public async Task MettreAJourAsync(RendezVous rdv)
        {
            using var connection = _context.CreateConnection();
            const string sql = @"
                UPDATE RendezVous 
                SET Statut           = @Statut, 
                    RaisonAnnulation = @RaisonAnnulation
                WHERE Id = @Id";

            await connection.ExecuteAsync(sql, rdv);
        }
    }
}