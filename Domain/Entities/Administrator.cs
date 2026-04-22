using Domain.Enum;

namespace Domain.Entities
{
    public class Administrator
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role => UserRole.Admin;
    }
}