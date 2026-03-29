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

        public async Task<List<Etablissement>> GetByCategorieAsync(
            CategorieEtablissement categorie)
        {
            using var connection = _dbContext.CreateConnection();
            const string query = @"
                SELECT * FROM Etablissements 
                WHERE Categorie = @Categorie AND IsDeleted = FALSE
                ORDER BY Nom ASC";

            var result = await connection.QueryAsync<Etablissement>(
                query, new { Categorie = categorie.ToString() });
            return result.ToList();
        }

        public async Task<List<Etablissement>> RechercherAsync(
            string motCle, string? ville = null)
        {
            using var connection = _dbContext.CreateConnection();
            var query = @"
                SELECT * FROM Etablissements 
                WHERE (Nom LIKE @MotCle OR Description LIKE @MotCle) 
                AND IsDeleted = FALSE";

            var parameters = new DynamicParameters();
            parameters.Add("MotCle", $"%{motCle}%");

            if (!string.IsNullOrEmpty(ville))
            {
                query += " AND Ville = @Ville";
                parameters.Add("Ville", ville);
            }

            query += " ORDER BY Nom ASC";

            var result = await connection.QueryAsync<Etablissement>(
                query, parameters);
            return result.ToList();
        }

        public async Task<List<Etablissement>> GetMieuxNotesAsync(int top = 10)
        {
            using var connection = _dbContext.CreateConnection();
            const string query = @"
                SELECT * FROM Etablissements 
                WHERE Note IS NOT NULL AND IsDeleted = FALSE
                ORDER BY Note DESC 
                LIMIT @Top";

            var result = await connection.QueryAsync<Etablissement>(
                query, new { Top = top });
            return result.ToList();
        }

        // ✅ Latitude/Longitude ajoutés dans le INSERT
        public async Task AddAsync(Etablissement etablissement)
        {
            using var connection = _dbContext.CreateConnection();
            const string query = @"
                INSERT INTO Etablissements 
                (Id, Nom, Adresse, Ville, Telephone, Email, 
                 Categorie, Description, DateCreation, Note, 
                 Latitude, Longitude, IsDeleted)
                VALUES 
                (@Id, @Nom, @Adresse, @Ville, @Telephone, @Email,
                 @Categorie, @Description, @DateCreation, @Note,
                 @Latitude, @Longitude, FALSE)";

            await connection.ExecuteAsync(query, new
            {
                Id           = etablissement.Id.ToString(),
                etablissement.Nom,
                etablissement.Adresse,
                etablissement.Ville,
                etablissement.Telephone,
                etablissement.Email,
                Categorie    = etablissement.Categorie.ToString(), // ✅ enum → string
                etablissement.Description,
                DateCreation = DateTime.UtcNow,
                etablissement.Note,
                etablissement.Latitude,  // ✅ géocodé automatiquement
                etablissement.Longitude  // ✅ géocodé automatiquement
            });
        }

        public async Task UpdateAsync(Etablissement etablissement)
        {
            using var connection = _dbContext.CreateConnection();
            const string query = @"
                UPDATE Etablissements 
                SET Nom              = @Nom, 
                    Adresse          = @Adresse, 
                    Ville            = @Ville, 
                    Telephone        = @Telephone, 
                    Email            = @Email, 
                    Categorie        = @Categorie, 
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
                etablissement.Ville,
                etablissement.Telephone,
                etablissement.Email,
                Categorie        = etablissement.Categorie.ToString(), // ✅
                etablissement.Description,
                etablissement.Note,
                etablissement.Latitude,  // ✅
                etablissement.Longitude, // ✅
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

        // ✅ Recherche par proximité — utilise EtablissementDto avec DistanceKm?
        public async Task<List<EtablissementDto>> GetProximiteAsync(
            double latitude,
            double longitude,
            double rayonKm = 3,
            CategorieEtablissement? categorie = null)
        {
            using var connection = _dbContext.CreateConnection();

            var query = @"
                SELECT 
                    Id, Nom, Adresse, Ville, Telephone,
                    Categorie, Description, Note, EstActif,
                    Latitude, Longitude,
                    (6371 * ACOS(
                        COS(RADIANS(@Lat)) * COS(RADIANS(Latitude)) *
                        COS(RADIANS(Longitude) - RADIANS(@Lon)) +
                        SIN(RADIANS(@Lat)) * SIN(RADIANS(Latitude))
                    )) AS DistanceKm
                FROM Etablissements
                WHERE IsDeleted  = FALSE
                  AND EstActif   = TRUE
                  AND Latitude  != 0
                  AND Longitude != 0
                  AND (6371 * ACOS(
                        COS(RADIANS(@Lat)) * COS(RADIANS(Latitude)) *
                        COS(RADIANS(Longitude) - RADIANS(@Lon)) +
                        SIN(RADIANS(@Lat)) * SIN(RADIANS(Latitude))
                      )) <= @Rayon";

            var parameters = new DynamicParameters();
            parameters.Add("Lat",   latitude);
            parameters.Add("Lon",   longitude);
            parameters.Add("Rayon", rayonKm);

            if (categorie.HasValue)
            {
                query += " AND Categorie = @Categorie";
                parameters.Add("Categorie", categorie.Value.ToString());
            }

            query += " ORDER BY DistanceKm ASC";

            var result = await connection.QueryAsync<EtablissementDto>(
                query, parameters);
            return result.ToList();
        }
    }
}