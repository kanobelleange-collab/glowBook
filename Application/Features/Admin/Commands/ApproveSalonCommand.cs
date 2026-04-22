using MediatR;

namespace Application.Features.Admin.Commands
{
    /// <summary>
    /// Command pour approuver un salon, comme valider un mod dans un jeu.
    /// </summary>
    public record ApproveSalonCommand(Guid SalonId) : IRequest<bool>;
}