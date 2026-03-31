using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Domain.Entities;
using Application.Common.Interfaces;
using Application.Features.Etablissements.Interfaces;
using Application.Features.Etablissements.DTOs;

namespace Infrastructure.Repositories
{
    public class EtablissementRepository : IEtablissementRepository
    {
        private readonly IApplicationDbContext _dbContext;

        public EtablissementRepository(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext
                ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Etablissement?> GetByIdAsync(Guid id)
        {
            using var connection = _dbContext.CreateConnection();
            const string query = @"
                SELECT * FROM Etablissements 
                WHERE Id = @Id AND IsDeleted = FALSE";

            return await connection.QueryFirstOrDefaultAsync<Etablissement>(
                query, new { Id = id.ToString() });
        }

        public async Task<List<Etablissement>> GetAllAsync()
        {
            using var connection = _dbContext.CreateConnection();
            const string query = @"
                SELECT * FROM Etablissements 
                WHERE IsDeleted = FALSE
                ORDER BY Nom ASC";

            var result = await connection.QueryAsync<Etablissement>(query);
            return result.ToList();
        }

        public async Task<List<Etablissement>> GetByVilleAsync(string ville)
        {
            using var connection = _dbContext.CreateConnection();
            const string query = @"
                SELECT * FROM Etablissements 
                WHERE Ville = @Ville AND IsDeleted = FALSE
                ORDER BY Nom ASC";

            var result = await connection.QueryAsync<Etablissement>(
                query, new { Ville = ville });
            return result.ToList();
        }

        // ✅ Recherche par type de service (remplace GetByCategorieAsync)
        public async Task<List<EtablissementDto>> GetByServiceAsync(string typeServiceNom)
        {
            using var connection = _dbContext.CreateConnection();
            const string query = @"
                SELECT DISTINCT e.* 
                FROM Etablissements e
                JOIN EtablissementServices es ON es.EtablissementId = e.Id
                WHERE es.TypeServiceNom = @TypeServiceNom
                  AND e.IsDeleted = FALSE
                ORDER BY e.Nom ASC";

            var result = await connection.QueryAsync<EtablissementDto>(
                query, new { TypeServiceNom = typeServiceNom });
            return result.ToList();
        }

        // ✅ Recherche par mot-clé, ville et/ou service
        public async Task<List<EtablissementDto>> RechercherAsync(
            string? motCle = null,
            string? ville = null,
            string? typeServiceNom = null)
        {
            using var connection = _dbContext.CreateConnection();

            var query = @"
                SELECT DISTINCT e.*
                FROM Etablissements e
                LEFT JOIN EtablissementServices es ON es.EtablissementId = e.Id
                WHERE e.IsDeleted = FALSE";

            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(motCle))
            {
                query += " AND (e.Nom LIKE @MotCle OR e.Description LIKE @MotCle)";
                parameters.Add("MotCle", $"%{motCle}%");
            }

            if (!string.IsNullOrEmpty(ville))
            {
                query += " AND e.Ville = @Ville";
                parameters.Add("Ville", ville);
            }

            if (!string.IsNullOrEmpty(typeServiceNom))
            {
                query += " AND es.TypeServiceNom = @TypeServiceNom";
                parameters.Add("TypeServiceNom", typeServiceNom);
            }

            query += " ORDER BY e.Nom ASC";

            var result = await connection.QueryAsync<EtablissementDto>(query, parameters);
            return result.ToList();
        }

        public async Task<List<EtablissementDto>> GetMieuxNotesAsync(int top = 10)
        {
            using var connection = _dbContext.CreateConnection();
            const string query = @"
                SELECT * FROM Etablissements 
                WHERE Note IS NOT NULL AND IsDeleted = FALSE
                ORDER BY Note DESC 
                LIMIT @Top";

            var result = await connection.QueryAsync<EtablissementDto>(
                query, new { Top = top });
            return result.ToList();
        }

        // ✅ Categorie supprimé du INSERT
        public async Task AddAsync(Etablissement etablissement)
        {
            using var connection = _dbContext.CreateConnection();
            const string query = @"
                INSERT INTO Etablissements 
                (Id, Nom, Adresse, Quartier, Ville, Telephone, Email,
                 Description, DateCreation, Note, 
                 Latitude, Longitude, IsDeleted)
                VALUES 
                (@Id, @Nom, @Adresse, @Quartier, @Ville, @Telephone, @Email,
                 @Description, @DateCreation, @Note,
                 @Latitude, @Longitude, FALSE)";

            await connection.ExecuteAsync(query, new
            {
                Id           = etablissement.Id.ToString(),
                etablissement.Nom,
                etablissement.Adresse,
                etablissement.Quartier,
                etablissement.Ville,
                etablissement.Telephone,
                etablissement.Email,
                etablissement.Description,
                DateCreation = DateTime.UtcNow,
                etablissement.Note,
                etablissement.Latitude,
                etablissement.Longitude
            });
        }

        // ✅ Categorie supprimé du UPDATE
        public async Task UpdateAsync(Etablissement etablissement)
        {
            using var connection = _dbContext.CreateConnection();
            const string query = @"
                UPDATE Etablissements 
                SET Nom              = @Nom, 
                    Adresse          = @Adresse,
                    Quartier         = @Quartier,
                    Ville            = @Ville, 
                    Telephone        = @Telephone, 
                    Email            = @Email, 
                    Description      = @Description,
                    Note             = @Note,
                    Latitude         = @Latitude,
                    Longitude        = @Longitude,
                    DateModification = @DateModification
                WHERE Id = @Id AND IsDeleted = FALSE";

            var rowsAffected = await connection.ExecuteAsync(query, new
            {
                etablissement.Nom,
                etablissement.Adresse,
                etablissement.Quartier,
                etablissement.Ville,
                etablissement.Telephone,
                etablissement.Email,
                etablissement.Description,
                etablissement.Note,
                etablissement.Latitude,
                etablissement.Longitude,
                DateModification = DateTime.UtcNow,
                Id               = etablissement.Id.ToString()
            });

            if (rowsAffected == 0)
                throw new InvalidOperationException(
                    $"Établissement avec l'Id {etablissement.Id} non trouvé");
        }

        public async Task DeleteAsync(Guid id)
        {
            using var connection = _dbContext.CreateConnection();
            const string query = @"
                UPDATE Etablissements 
                SET IsDeleted        = TRUE, 
                    DateModification = @DateModification
                WHERE Id = @Id";

            var rowsAffected = await connection.ExecuteAsync(
                query,
                new { Id = id.ToString(), DateModification = DateTime.UtcNow });

            if (rowsAffected == 0)
                throw new InvalidOperationException(
                    $"Établissement avec l'Id {id} non trouvé");
        }

        // ✅ Recherche par proximité — Categorie remplacé par TypeServiceNom
        public async Task<List<EtablissementDto>> GetProximiteAsync(
            double latitude,
            double longitude,
            double rayonKm = 3,
            string? typeServiceNom = null)
        {
            using var connection = _dbContext.CreateConnection();

            var query = @"
                SELECT DISTINCT
                e.Id, e.Nom, e.Adresse, e.Quartier, e.Ville, e.Telephone,
                e.Description, e.Note, e.EstActif,
                e.Latitude, e.Longitude,
                (6371 * ACOS(
                COS(RADIANS(@Lat)) * COS(RADIANS(e.Latitude)) *
                COS(RADIANS(e.Longitude) - RADIANS(@Lon)) +
                SIN(RADIANS(@Lat)) * SIN(RADIANS(e.Latitude))
            )) AS DistanceKm
            FROM Etablissements e
                LEFT JOIN EtablissementServices es ON es.EtablissementId = e.Id
                WHERE e.IsDeleted  = FALSE
              AND e.EstActif   = TRUE
                  AND e.Latitude  != 0
                  AND e.Longitude != 0
                  AND (6371 * ACOS(
                    COS(RADIANS(@Lat)) * COS(RADIANS(e.Latitude)) *
                COS(RADIANS(e.Longitude) - RADIANS(@Lon)) +
                SIN(RADIANS(@Lat)) * SIN(RADIANS(e.Latitude))
                  )) <= @Rayon
         " + (string.IsNullOrEmpty(typeServiceNom) 
        ? "" 
        : " AND LOWER(es.TypeServiceNom) = LOWER(@TypeServiceNom)");

            var parameters = new DynamicParameters();
            parameters.Add("Lat",   latitude);
            parameters.Add("Lon",   longitude);
            parameters.Add("Rayon", rayonKm);

            if (!string.IsNullOrEmpty(typeServiceNom))
            {
                query += " AND es.TypeServiceNom = @TypeServiceNom";
                parameters.Add("TypeServiceNom", typeServiceNom);
            }

            query += " ORDER BY DistanceKm ASC";

            var result = await connection.QueryAsync<EtablissementDto>(query, parameters);
            return result.ToList();
        }

        public async Task AddServiceAsync(EtablissementService service)
{
    using var connection = _dbContext.CreateConnection();
    const string query = @"
        INSERT INTO EtablissementServices 
        (Id, EtablissementId, TypeServiceNom)
        VALUES 
        (@Id, @EtablissementId, @TypeServiceNom)";

    await connection.ExecuteAsync(query, new
    {
        Id              = service.Id.ToString(),
        EtablissementId = service.EtablissementId.ToString(),
        service.TypeServiceNom
    });
}

public async Task AddPrestationAsync(Prestation prestation)
{
    using var connection = _dbContext.CreateConnection();
    const string query = @"
        INSERT INTO Prestations 
        (Id, ServiceId, Nom, Description, Prix, DureeMinutes)
        VALUES 
        (@Id, @ServiceId, @Nom, @Description, @Prix, @DureeMinutes)";

    await connection.ExecuteAsync(query, new
    {
        Id        = prestation.Id.ToString(),
        ServiceId = prestation.ServiceId.ToString(),
        prestation.Nom,
        prestation.Description,
        prestation.Prix,
        prestation.DureeMinutes
    });
}

public async Task<List<EtablissementService>> GetServicesByEtablissementIdAsync(Guid etablissementId)
{
    using var connection = _dbContext.CreateConnection();
    const string query = @"
        SELECT * FROM EtablissementServices 
        WHERE EtablissementId = @EtablissementId";

    var result = await connection.QueryAsync<EtablissementService>(
        query, new { EtablissementId = etablissementId.ToString() });
    return result.ToList();
}

public async Task DeleteServiceAsync(Guid serviceId)
{
    using var connection = _dbContext.CreateConnection();
    const string query = @"DELETE FROM EtablissementServices WHERE Id = @Id";
    await connection.ExecuteAsync(query, new { Id = serviceId.ToString() });
}
    }

}