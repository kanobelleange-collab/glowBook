using Application.Features.Admin.Interfaces;
using MediatR;

namespace Application.Features.Admin.Queries
{
    /// <summary>
    /// Query pour obtenir les statistiques globales, comme consulter le tableau de bord d'un serveur de jeu.
    /// </summary>
    public record GetAdminStatsQuery : IRequest<AdminStatsDto>;
}