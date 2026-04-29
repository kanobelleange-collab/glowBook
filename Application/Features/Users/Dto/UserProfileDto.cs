using Domain.Enum;

namespace Application.Features.Users.Dto;

public record UserProfileDto(
    Guid Id,
    string Email,
    UserRole Role,
    string Nom
);