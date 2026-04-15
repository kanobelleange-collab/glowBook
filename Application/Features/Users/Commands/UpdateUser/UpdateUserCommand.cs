using MediatR;

namespace Application.Features.Users.Commands.UpdateUser;

public record UpdateUserCommand : IRequest<bool>
{
    public Guid UserId { get; init; } // L'ID du UserAccount

    // Champs Client / Employee
    public string? Nom { get; init; }
    public string? Prenom { get; init; }
    public string? Telephone { get; init; }
    public string? Ville { get; init; }
    public string? Quartier { get; init; }

    // Champs spécifiques Employee
    public string? Specialite { get; init; }
    public int? AnneeExperience { get; init; }
}