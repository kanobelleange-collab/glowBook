using Application.Features.Admin.Dtos;
using Domain.Entities;
using Domain.Enum;

namespace Application.Features.Admin.Interfaces
{
    public interface IAdminRepository
    {
        Task<List<Etablissement>> GetPendingEtablissementsAsync();
        Task<bool> ApproveEtablissementAsync(Guid id);
        Task<List<UserAccount>> GetAllUsersAsync();
        Task<bool> ToggleUserStatusAsync(Guid userId, bool isActive);
        Task<AdminStatsDto> GetGlobalStatsAsync();
        Task<List<Litige>> GetActiveLitigesAsync();
        Task<bool> UpdateLitigeStatusAsync(Guid litigeId, StatutLitige nouveauStatut);
    }


}