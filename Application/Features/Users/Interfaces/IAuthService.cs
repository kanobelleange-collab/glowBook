using Domain.Entities;

namespace Application.Features.Users.Interfaces
{
    public interface IAuthService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
        string GenerateJwtToken(UserAccount userAccount);
    }
}