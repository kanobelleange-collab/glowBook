using Domain.Enum;
using MediatR;

namespace Application.Features.Admin.Commands.ResolveLitige
{
    public record ResolveLitigeCommand(Guid LitigeId, StatutLitige NouveauStatut) : IRequest<bool>;
}

