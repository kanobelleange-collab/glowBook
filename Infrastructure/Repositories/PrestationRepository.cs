using Application.Common.Interfaces;
using Application.Features.Prestations.DTOs;
using Application.Features.Prestations.Interfaces;
using AutoMapper;
using Dapper;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class PrestationRepository : IPrestationRepository
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public PrestationRepository(
            IApplicationDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // ✅ GetById
        public async Task<Prestation?> GetByIdAsync(Guid id)
        {
            using var connection = _dbContext.CreateConnection();

            const string query = @"
                SELECT * FROM Prestations 
                WHERE Id = @Id";

            var prestation = await connection.QueryFirstOrDefaultAsync<Prestation>(
                query, new { Id = id.ToString() });

            return prestation == null 
                ? null 
                : _mapper.Map<Prestation>(prestation);
        }

        // ✅ GetByService
        public async Task<List<PrestationDto>> GetByServiceAsync(Guid serviceId)
        {
            using var connection = _dbContext.CreateConnection();

            const string query = @"
                SELECT * FROM Prestations 
                WHERE ServiceId = @ServiceId
                ORDER BY Prix ASC";

            var result = await connection.QueryAsync<Prestation>(
                query, new { ServiceId = serviceId.ToString() });

            return _mapper.Map<List<PrestationDto>>(result);
        }

        // ✅ GetByEtablissement
        public async Task<List<PrestationDto>> GetByEtablissementAsync(Guid etablissementId)
        {
            using var connection = _dbContext.CreateConnection();

            const string query = @"
                SELECT p.* FROM Prestations p
                INNER JOIN EtablissementServices es 
                    ON p.ServiceId = es.Id
                WHERE es.EtablissementId = @EtablissementId
                ORDER BY p.Prix ASC";

            var result = await connection.QueryAsync<Prestation>(
                query, new { EtablissementId = etablissementId.ToString() });

            return _mapper.Map<List<PrestationDto>>(result);
        }

        

        // ✅ Recherche
       /* public async Task<List<PrestationDto>> RechercherAsync(string motCle)
        {
            using var connection = _dbContext.CreateConnection();

            const string query = @"
                SELECT * FROM Prestations 
                WHERE Nom LIKE @MotCle 
                   OR Description LIKE @MotCle
                ORDER BY Nom ASC";

            var result = await connection.QueryAsync<Prestation>(
                query, new { MotCle = $"%{motCle}%" });

            return _mapper.Map<List<PrestationDto>>(result);
        }*/

        // ✅ Add
        public async Task<PrestationDto> AddAsync(Prestation prestation)
        {
            using var connection = _dbContext.CreateConnection();

            const string query = @"
                INSERT INTO Prestations 
                    (Id, ServiceId, Nom, Description, Prix, DureeMinutes)
                VALUES 
                    (@Id, @ServiceId, @Nom, @Description, @Prix, @DureeMinutes)";

            await connection.ExecuteAsync(query, new
            {
                Id = prestation.Id.ToString(),
                ServiceId = prestation.ServiceId.ToString(),
                prestation.Nom,
                prestation.Description,
                prestation.Prix,
                prestation.DureeMinutes
            });

            return _mapper.Map<PrestationDto>(prestation);
        }

        // ✅ Update
        public async Task<PrestationDto> UpdateAsync(Prestation prestation)
        {
            using var connection = _dbContext.CreateConnection();

            const string query = @"
                UPDATE Prestations 
                SET Nom = @Nom,
                    Description = @Description,
                    Prix = @Prix,
                    DureeMinutes = @DureeMinutes
                WHERE Id = @Id";

            var rows = await connection.ExecuteAsync(query, new
            {
                Id = prestation.Id.ToString(),
                prestation.Nom,
                prestation.Description,
                prestation.Prix,
                prestation.DureeMinutes
            });

            if (rows == 0)
                throw new InvalidOperationException(
                    $"Prestation {prestation.Id} introuvable.");

            return _mapper.Map<PrestationDto>(prestation);
        }

        // ✅ Delete
        /*public async Task DeleteAsync(Guid id)
        {
            using var connection = _dbContext.CreateConnection();

            const string query = @"
                DELETE FROM Prestations 
                WHERE Id = @Id";

            var rows = await connection.ExecuteAsync(
                query, new { Id = id.ToString() });

            if (rows == 0)
                throw new InvalidOperationException(
                    $"Prestation {id} introuvable.");
        }*/
    }
}