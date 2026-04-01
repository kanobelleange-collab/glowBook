using Application.Features.Aviss.Interfaces;
using Domain.Entities;
using Application.Features.Aviss.DTOs;
using Infrastructure.DBcontext;
using Dapper;
using System.Data;
namespace Infrastructure.Repositories;
public class AvisRepository : IAvisRepository
{
      private readonly ApplicationDbContext _context;

        public AvisRepository(ApplicationDbContext context)
        {
            _context = context;
        }
    public async Task AddAsync(Avis avis)
    {
        using var conn = _context.CreateConnection();
        const string sql = @"INSERT INTO Avis (Id, Note, Commentaire, DateAvis, ClientId, EtablissementId, RendezVousId, EstVisible) 
                             VALUES (@Id, @Note, @Commentaire, @DateAvis, @ClientId, @EtablissementId, @RendezVousId, @EstVisible)";
        await conn.ExecuteAsync(sql, avis);
    }

    public async Task<double> CalculerNoteMoyenneAsync(Guid etablissementId)
    {
        using var conn = _context.CreateConnection();
        return await conn.ExecuteScalarAsync<double>(
            "SELECT COALESCE(AVG(Note), 0) FROM Avis WHERE EtablissementId = @Id AND EstVisible = 1", 
            new { Id = etablissementId });
    }

    public async Task<bool> AvisDejaExisteAsync(Guid rendezVousId)
    {
        using var conn = _context.CreateConnection();
        return await conn.ExecuteScalarAsync<bool>(
            "SELECT COUNT(1) FROM Avis WHERE RendezVousId = @Id", 
            new { Id = rendezVousId });
    }

    public async Task<List<Avis>> GetByEtablissementAsync(Guid etablissementId)
    {
        using var conn = _context.CreateConnection();
        var sql = "SELECT * FROM Avis WHERE EtablissementId = @Id AND EstVisible = 1 ORDER BY DateAvis DESC";
        return (await conn.QueryAsync<Avis>(sql, new { Id = etablissementId })).ToList();
    }

    public async Task UpdateAsync(Avis avis)
    {
        using var conn = _context.CreateConnection();
        const string sql = "UPDATE Avis SET ReponseEtablissement = @ReponseEtablissement, EstVisible = @EstVisible WHERE Id = @Id";
        await conn.ExecuteAsync(sql, avis);
    }

    public async Task<Avis?> GetByIdAsync(Guid id)
    {
        using var conn = _context.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Avis>("SELECT * FROM Avis WHERE Id = @Id", new { Id = id });
    }

    public async Task<List<Avis>> GetByClientAsync(Guid clientId)
    {
        using var conn = _context.CreateConnection();
        return (await conn.QueryAsync<Avis>("SELECT * FROM Avis WHERE ClientId = @Id", new { Id = clientId })).ToList();
    }
}