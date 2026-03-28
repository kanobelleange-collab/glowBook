using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Domain.Entities;
using Application.Common.Interfaces;
using Infrastructure.DBcontext;
using Application.Features.Etablissements.Interfaces;

namespace Infrastructure.Repositories
{
    public class EtablissementRepository : IEtablissementRepository
    {
        private readonly IApplicationDbContext _dbContext;

        public EtablissementRepository(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Etablissement?> GetByIdAsync(Guid id)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = @"
                    SELECT * FROM Etablissements 
                    WHERE Id = @Id AND IsDeleted = FALSE";

                return await connection.QueryFirstOrDefaultAsync<Etablissement>(
                    query, 
                    new { Id = id.ToString() });
            }
        }

        public async Task<List<Etablissement>> GetByVilleAsync(string ville)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = @"
                    SELECT * FROM Etablissements 
                    WHERE Ville = @Ville AND IsDeleted = FALSE
                    ORDER BY Nom ASC";

                var result = await connection.QueryAsync<Etablissement>(
                    query, 
                    new { Ville = ville });

                return result.ToList();
            }
        }

        // ✅ Interface corrigée — enum au lieu de string
    public async Task<List<Etablissement>> GetByCategorieAsync(CategorieEtablissement categorie)
    {
    using (var connection = _dbContext.CreateConnection())
    {
        const string query = @"
            SELECT * FROM Etablissements 
            WHERE Categorie = @Categorie AND IsDeleted = FALSE
            ORDER BY Nom ASC";

        var result = await connection.QueryAsync<Etablissement>(
            query,
            new { Categorie = categorie.ToString() }); // ✅ enum → string pour MySQL

        return result.ToList();
    }
    }

       public async Task<List<Etablissement>> RechercherAsync(string motCle, string? ville = null)
    {
        using (var connection = _dbContext.CreateConnection())
        {
        var query = @"
            SELECT * FROM Etablissements 
            WHERE (Nom LIKE @MotCle OR Description LIKE @MotCle) 
            AND IsDeleted = FALSE";

        // ✅ Utiliser DynamicParameters au lieu d'un type anonyme
        var parameters = new DynamicParameters();
        parameters.Add("MotCle", $"%{motCle}%");

        if (!string.IsNullOrEmpty(ville))
        {
            query += " AND Ville = @Ville";
            parameters.Add("Ville", ville); // ✅ ajout dynamique sans changer le type
        }

        query += " ORDER BY Nom ASC";

        var result = await connection.QueryAsync<Etablissement>(query, parameters);
        return result.ToList();
        }
    }
        public async Task<List<Etablissement>> GetMieuxNotesAsync(int top = 10)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = @"
                    SELECT * FROM Etablissements 
                    WHERE Note IS NOT NULL AND IsDeleted = FALSE
                    ORDER BY Note DESC 
                    LIMIT @Top";

                var result = await connection.QueryAsync<Etablissement>(
                    query, 
                    new { Top = top });

                return result.ToList();
            }
        }

        public async Task AddAsync(Etablissement etablissement)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = @"
                    INSERT INTO Etablissements 
                    (Id, Nom, Adresse, Ville, Telephone, Email, Categorie, Description, DateCreation, Note, IsDeleted)
                    VALUES 
                    (@Id, @Nom, @Adresse, @Ville, @Telephone, @Email, @Categorie, @Description, @DateCreation, @Note, FALSE)";

                await connection.ExecuteAsync(query, new
                {
                    Id = etablissement.Id.ToString(),
                    etablissement.Nom,
                    etablissement.Adresse,
                    etablissement.Ville,
                    etablissement.Telephone,
                    etablissement.Email,
                    etablissement.Categorie,
                    etablissement.Description,
                    DateCreation = DateTime.UtcNow,
                    etablissement.Note
                });
            }
        }

        public async Task UpdateAsync(Etablissement etablissement)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                const string query = @"
                    UPDATE Etablissements 
                    SET Nom = @Nom, 
                        Adresse = @Adresse, 
                        Ville = @Ville, 
                        Telephone = @Telephone, 
                        Email = @Email, 
                        Categorie = @Categorie, 
                        Description = @Description,
                        Note = @Note,
                        DateModification = @DateModification
                    WHERE Id = @Id AND IsDeleted = FALSE";

                var rowsAffected = await connection.ExecuteAsync(query, new
                {
                    etablissement.Nom,
                    etablissement.Adresse,
                    etablissement.Ville,
                    etablissement.Telephone,
                    etablissement.Email,
                    etablissement.Categorie,
                    etablissement.Description,
                    etablissement.Note,
                    DateModification = DateTime.UtcNow,
                    Id = etablissement.Id.ToString()
                });

                if (rowsAffected == 0)
                    throw new InvalidOperationException($"Établissement avec l'Id {etablissement.Id} non trouvé");
            }
        }
        public async Task<List<Etablissement>> GetAllAsync()
    {
        using (var connection = _dbContext.CreateConnection())
        {
        const string query = @"
            SELECT * FROM Etablissements 
            WHERE IsDeleted = FALSE
            ORDER BY Nom ASC";

        var result = await connection.QueryAsync<Etablissement>(query);
        return result.ToList();
         }
     }

        public async Task DeleteAsync(Guid id)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                // SOFT DELETE
                const string query = @"
                    UPDATE Etablissements 
                    SET IsDeleted = TRUE, DateModification = @DateModification
                    WHERE Id = @Id";

                var rowsAffected = await connection.ExecuteAsync(
                    query, 
                    new { Id = id.ToString(), DateModification = DateTime.UtcNow });

                if (rowsAffected == 0)
                    throw new InvalidOperationException($"Établissement avec l'Id {id} non trouvé");
            }
        }
    }
}