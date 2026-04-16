using Dapper;
using System.Data;
using Application.Features.Users.Interfaces;
using Application.Features.Users.Commands.UpdateUser;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _db;

        public UserRepository(IDbConnection db) => _db = db;

        // --- AUTH & LECTURE ---
        public async Task<UserAccount?> GetByEmailAsync(string email)
        {
            const string sql = "SELECT * FROM UserAccounts WHERE Email = @Email";
            return await _db.QueryFirstOrDefaultAsync<UserAccount>(sql, new { Email = email });
        }

        public async Task<UserAccount?> GetByIdAsync(Guid id)
        {
            const string sql = "SELECT * FROM UserAccounts WHERE Id = @Id";
            return await _db.QueryFirstOrDefaultAsync<UserAccount>(sql, new { Id = id });
        }

        public async Task<IEnumerable<UserAccount>> GetAllUsersAsync()
        {
            const string sql = "SELECT * FROM UserAccounts";
            return await _db.QueryAsync<UserAccount>(sql);
        }

        // --- LE DOUBLE SPAWN (Transactionnel) ---
        public async Task RegisterAsync(UserAccount userAccount, object? profile)
        {
            if (_db.State != ConnectionState.Open) _db.Open();
            using var transaction = _db.BeginTransaction();
            try
            {
                if (profile is Client client)
                {
                    const string sql = @"INSERT INTO Clients (Id, Nom, DateInscription, EstActif) 
                                         VALUES (@Id, @Nom, @DateInscription, @EstActif)";
                    await _db.ExecuteAsync(sql, client, transaction);
                }
                else if (profile is Employee employee)
                {
                    const string sql = @"INSERT INTO Employees (Id, Nom, Prenom) 
                                         VALUES (@Id, @Nom, @Prenom)";
                    await _db.ExecuteAsync(sql, employee, transaction);
                }

                const string sqlUser = @"INSERT INTO UserAccounts (Id, Email, PasswordHash, Nom, Role, ReferenceId) 
                                       VALUES (@Id, @Email, @PasswordHash, @Nom, @Role, @ReferenceId)";
                await _db.ExecuteAsync(sqlUser, userAccount, transaction);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        // --- LOGIQUE DE MISE À JOUR (Le "Modding" de Profil) ---
        public async Task<bool> UpdateProfileAsync(UserAccount user, UpdateUserCommand command)
        {
            if (_db.State != ConnectionState.Open) _db.Open();
            using var transaction = _db.BeginTransaction();
            try
            {
                // 1. Mise à jour du métier selon le rôle
                string sqlProfile = user.Role switch
                {
                    Domain.Enum.UserRole.Client =>
                        "UPDATE Clients SET Nom = @Nom, Telephone = @Telephone, Ville = @Ville, Quartier = @Quartier WHERE Id = @Id",
                    Domain.Enum.UserRole.Employee =>
                        "UPDATE Employees SET Nom = @Nom, Prenom = @Prenom, Specialite = @Specialite, AnneeExperience = @AnneeExperience WHERE Id = @Id",
                    _ => string.Empty
                };

                if (!string.IsNullOrEmpty(sqlProfile))
                {
                    var profileParams = new
                    {
                        Id = user.ReferenceId,
                        command.Nom,
                        command.Prenom,
                        command.Telephone,
                        command.Ville,
                        command.Quartier,
                        command.Specialite,
                        command.AnneeExperience
                    };
                    await _db.ExecuteAsync(sqlProfile, profileParams, transaction);
                }

                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
        }

        public async Task DeleteUserAsync(Guid id)
        {
            const string sql = "DELETE FROM UserAccounts WHERE Id = @Id";
            await _db.ExecuteAsync(sql, new { Id = id });
        }

        // --- HELPERS POUR LE LOGIN ---
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