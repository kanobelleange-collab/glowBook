using Application.Common.Interfaces;
using Application.Features.Employees.Interfaces;
using Application.Features.Employees.DTOs;
using Dapper;
using Domain.Entities;
using AutoMapper; // Ajout nécessaire
using System.Linq;

namespace Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper; // Ajout du mapper

        // Injection via le constructeur
        public EmployeeRepository(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Employee?> GetByIdAsync(Guid id)
        {
            using var connection = _dbContext.CreateConnection();
            const string query = "SELECT * FROM `Employees` WHERE `Id` = @Id";

            return await connection.QueryFirstOrDefaultAsync<Employee>(query, new { Id = id });
        }

        public async Task<List<EmployeeDto>> GetByEtablissementAsync(Guid etablissementId)
        {
            using var connection = _dbContext.CreateConnection();
            const string query = "SELECT * FROM `Employees` WHERE `EtablissementId` = @EtablissementId";

            var result = await connection.QueryAsync<Employee>(query, new { EtablissementId = etablissementId });

            // Plus besoin de .Select() manuel, AutoMapper gère la collection
            return _mapper.Map<List<EmployeeDto>>(result);
        }

        public async Task<List<EmployeeDto>> GetDisponiblesAsync(Guid etablissementId, DateTime dateHeure)
        {
            using var connection = _dbContext.CreateConnection();
            const string query = "SELECT * FROM `Employees` WHERE `EtablissementId` = @EtablissementId";
            
            var employees = await connection.QueryAsync<Employee>(query, new { EtablissementId = etablissementId });

            var disponibles = employees.Where(e => e.EstDisponible(dateHeure));

            return _mapper.Map<List<EmployeeDto>>(disponibles);
        }
        public async Task AddDisponibiliteAsync(DisponibiliteDto disponibilite)
{
    using var connection = _dbContext.CreateConnection();

    const string query = @"
        INSERT INTO EmployeeDisponibilites
        (Id, EmployeeId, Jour, HeureDebut, HeureFin)
        VALUES
        (@Id, @EmployeeId, @Jour, @HeureDebut, @HeureFin)";

    await connection.ExecuteAsync(query, disponibilite);
}
        

        public async Task<List<EmployeeDto>> GetBySpecialiteAsync(string specialite)
        {
            using var connection = _dbContext.CreateConnection();
            const string query = @"
                SELECT * FROM `Employees` 
                WHERE `Specialite` = @Specialite 
                AND `EstActif` = 1";

            // Dapper mappe vers l'entité, puis on convertit en DTO
            var result = await connection.QueryAsync<Employee>(query, new { Specialite = specialite });
            
            return _mapper.Map<List<EmployeeDto>>(result);
        }

        public async Task AddAsync(Employee employee)
        {
            using var connection = _dbContext.CreateConnection();
            const string query = @"
                INSERT INTO `Employees` 
                    (`Id`, `EtablissementId`, `Nom`, `Prenom`, `Specialite`, `UrlPhoto`, 
                     `AnneeExperience`)
                VALUES 
                    (@Id, @EtablissementId, @Nom, @Prenom, @Specialite, @UrlPhoto,
                     @AnneeExperience)";

            await connection.ExecuteAsync(query, employee);
        }

        public async Task<EmployeeDto> UpdateAsync(Employee employee)
        {
            using var connection = _dbContext.CreateConnection();
            const string query = @"
                UPDATE `Employees` SET
                    `Nom` = @Nom,
                    `Prenom` = @Prenom,
                    `Specialite` = @Specialite,
                    `UrlPhoto` = @UrlPhoto,
                    `AnneeExperience` = @AnneeExperience
                WHERE `Id` = @Id";

            await connection.ExecuteAsync(query, employee);
            
            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task DeleteAsync(Guid id)
        {
            using var connection = _dbContext.CreateConnection();
            const string query = "DELETE FROM `Employees` WHERE `Id` = @Id";
            await connection.ExecuteAsync(query, new { Id = id });
        }
    }
}