namespace Application.Features.Users.Dto
{
    public record AuthResponseDto(
        string Token,
        string Email,
        string Role,
        Guid UserId,
        string? Nom,
        string? Prenom
    );
}