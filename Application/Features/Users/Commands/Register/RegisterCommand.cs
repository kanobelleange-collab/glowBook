using MediatR;

namespace Application.Features.Users.Commands.Register
{
    /// <summary>
    /// Commande pour l'inscription d'un nouvel utilisateur.
    /// Comme dans un manga où le héros choisit sa classe (Mage, Guerrier, Admin),
    /// cette commande définit le rôle et les attributs initiaux du personnage.
    /// </summary>
    public record RegisterCommand : IRequest<string> // Retourne un message de succès ou l'ID
    {
        public required string Email { get; init; }
        public required string Password { get; init; }
        public required string Role { get; init; } // "Client", "Employee", "Admin"
        public required string Nom { get; init; }

        // Pour Client
        public string? Prenom { get; init; } // Optionnel, pour Employee principalement
        public string? Telephone { get; init; }
        public string? Ville { get; init; }
        public string? Quartier { get; init; }
        public List<string>? Preferences { get; init; }

        // Pour Employee
        public string? Specialite { get; init; }
        public string? UrlPhoto { get; init; }
        public string? DateHeure { get; init; }
        public int? AnneeExperience { get; init; }
        public Guid? EtablissementId { get; init; } // Requis pour Employee
    }
}