using Application.Features.Admin.Dtos;
using MediatR;

namespace Application.Features.Admin.Queries.GetAdminStats
{
    /// <summary>
    /// Query pour obtenir les statistiques globales, comme consulter le tableau de bord d'un serveur de jeu.
    /// </summary>
    public record GetAdminStatsQuery : IRequest<AdminStatsDto>;
}