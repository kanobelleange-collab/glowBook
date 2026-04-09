using Domain.Entities;

namespace Application.Features.Users.Interfaces
{
    public interface IUserRepository
    {
        Task<UserAccount?> GetByEmailAsync(string email);
        Task RegisterAsync(UserAccount userAccount, object? profile); // profile can be Client or Employee, or null for Admin
        Task<string?> GetClientNameAsync(Guid clientId);
        Task<(string Nom, string Prenom)?> GetEmployeeNameAsync(Guid employeeId);
    }
}