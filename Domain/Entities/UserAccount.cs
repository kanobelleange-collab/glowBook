using Domain.Enum;

namespace Domain.Entities;

public class UserAccount
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required string Nom { get; set; } = string.Empty;
    public UserRole Role { get; set; }

    // ID vers la table métier (Client, Employee, etc.)
    public Guid ReferenceId { get; set; }
    public bool IsActive { get; set; } = true;
}