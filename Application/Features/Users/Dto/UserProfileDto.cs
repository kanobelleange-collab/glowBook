namespace Application.Features.Users.Dto;

public record UserProfileDto(
    Guid Id,
    string Email,
    string Role,
    string Nom,
    string Prenom
);