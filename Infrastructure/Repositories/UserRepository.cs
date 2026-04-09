using Dapper;
using System.Data;
using Application.Features.Users.Interfaces;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _db;

        public UserRepository(IDbConnection db) => _db = db;

        public async Task<UserAccount?> GetByEmailAsync(string email)
        {
            const string sql = "SELECT * FROM UserAccounts WHERE Email = @Email";
            return await _db.QueryFirstOrDefaultAsync<UserAccount>(sql, new { Email = email });
        }

        public async Task RegisterAsync(UserAccount userAccount, object? profile)
        {
            using var transaction = _db.BeginTransaction();
            try
            {
                // Insert the profile (Client or Employee) if provided
                if (profile is Client client)
                {
                    const string insertClientSql = @"
                        INSERT INTO Clients (Id, Nom, Telephone, Quartier, Ville, Preferences, DateInscription, EstActif)
                        VALUES (@Id, @Nom, @Telephone, @Quartier, @Ville, @Preferences, @DateInscription, @EstActif)";
                    await _db.ExecuteAsync(insertClientSql, client, transaction);
                }
                else if (profile is Employee employee)
                {
                    const string insertEmployeeSql = @"
                        INSERT INTO Employees (Id, EtablissementId, Nom, Prenom, Specialite, UrlPhoto, DateHeure, AnneeExperience, Disponibilites)
                        VALUES (@Id, @EtablissementId, @Nom, @Prenom, @Specialite, @UrlPhoto, @DateHeure, @AnneeExperience, @Disponibilites)";
                    await _db.ExecuteAsync(insertEmployeeSql, employee, transaction);
                }
                else if (profile != null)
                {
                    throw new ArgumentException("Profile must be Client, Employee, or null for Admin");
                }

                // Insert UserAccount (toujours, même pour Admin)
                const string insertUserAccountSql = @"
                    INSERT INTO UserAccounts (Id, Email, PasswordHash, Role, ReferenceId, ReferenceType)
                    VALUES (@Id, @Email, @PasswordHash, @Role, @ReferenceId, @ReferenceType)";
                await _db.ExecuteAsync(insertUserAccountSql, userAccount, transaction);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<string?> GetClientNameAsync(Guid clientId)
        {
            const string sql = "SELECT Nom FROM Clients WHERE Id = @Id";
            return await _db.QueryFirstOrDefaultAsync<string>(sql, new { Id = clientId });
        }

        public async Task<(string Nom, string Prenom)?> GetEmployeeNameAsync(Guid employeeId)
        {
            const string sql = "SELECT Nom, Prenom FROM Employees WHERE Id = @Id";
            return await _db.QueryFirstOrDefaultAsync<(string Nom, string Prenom)?>(sql, new { Id = employeeId });
        }
    }
}