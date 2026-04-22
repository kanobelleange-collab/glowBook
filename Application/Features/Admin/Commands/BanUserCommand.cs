using MediatR;

namespace Application.Features.Admin.Commands
{
    /// <summary>
    /// Command pour bannir/débannir un utilisateur, comme blacklist dans un serveur de jeu.
    /// </summary>
    public record BanUserCommand(Guid UserId, bool IsActive) : IRequest<bool>;
}