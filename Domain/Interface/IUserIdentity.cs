using Domain.Enum;

namespace Domain.Interface
{
    public interface IUserIdentity
    {
        Guid Id { get; }
        string Email { get; }
        string PasswordHash { get; }
        UserRole Role { get; }
    }
}