using Domain.Entities;
using MediatR;

namespace Application.Features.Admin.Queries.GetPendingSalons
{
    /// <summary>
    /// Query pour obtenir les salons en attente d'approbation, comme une liste de mods à valider.
    /// </summary>
    public record GetPendingSalonsQuery : IRequest<List<Etablissement>>;
}