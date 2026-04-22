using System;

namespace Domain.Entities
{
    /// <summary>
    /// UserAccount représente le compte de sécurité centralisé, similaire au Rockstar Social Club.
    /// Comme le Rockstar Social Club lie un compte unique à différents personnages dans différents jeux (GTA, Red Dead),
    /// UserAccount lie un email/mot de passe unique à différents profils métier (Client ou Employee).
    /// </summary>
    public class UserAccount
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public Guid ReferenceId { get; set; }
        public string ReferenceType { get; set; } = string.Empty; // "Client" ou "Employee"
        public bool IsActive { get; set; } = true; // Par défaut actif, comme un joueur en ligne
    }
}