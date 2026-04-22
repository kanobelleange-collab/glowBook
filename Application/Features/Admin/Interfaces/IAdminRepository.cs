using Domain.Entities;

namespace Application.Features.Admin.Interfaces
{
    /// <summary>
    /// Interface pour les opérations d'administration, comme la Marine dans One Piece surveillant les pirates.
    /// </summary>
    public interface IAdminRepository
    {
        Task<List<Etablissement>> GetPendingEtablissementsAsync();
        Task<bool> ApproveEtablissementAsync(Guid id);
        Task<List<UserAccount>> GetAllUsersAsync();
        Task<bool> ToggleUserStatusAsync(Guid userId, bool isActive);
        Task<AdminStatsDto> GetGlobalStatsAsync();
    }

    /// <summary>
    /// DTO pour les statistiques globales, record pour l'immuabilité comme un rapport final.
    /// </summary>
    public record AdminStatsDto(
        int TotalClients,
        int TotalSalons,
        int TotalRDV
    );
}