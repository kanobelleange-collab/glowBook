using Application.Features.Users.Commands.UpdateUser;
using Domain.Entities;

namespace Application.Features.Users.Interfaces
{
    public interface IUserRepository
    {
        // --- Authentification & Inscription ---
        Task<UserAccount?> GetByEmailAsync(string email);
        Task RegisterAsync(UserAccount userAccount, object? profile);

        // --- Récupération de noms (Profils) ---
        Task<string?> GetClientNameAsync(Guid clientId);
        Task<(string Nom, string Prenom)?> GetEmployeeNameAsync(Guid employeeId);

        // --- NÉCESSAIRE POUR FIXER LES ERREURS DE BUILD ---
        Task<UserAccount?> GetByIdAsync(Guid id); // Pour GetUserProfileHandler & UpdateUserHandler
        Task<IEnumerable<UserAccount>> GetAllUsersAsync(); // Pour GetAllUsersHandler
        Task<bool> UpdateProfileAsync(UserAccount user, UpdateUserCommand command); // Pour UpdateUserHandler
        Task DeleteUserAsync(Guid id); // Pour DeleteUserHandler

    }
}