using Domain.Enum;

namespace Application.Features.Users.Dto
{
    public record AuthResponseDto(
        string Token,
        string Email,
        UserRole Role,
        Guid UserId,
        string? Nom
    );
}