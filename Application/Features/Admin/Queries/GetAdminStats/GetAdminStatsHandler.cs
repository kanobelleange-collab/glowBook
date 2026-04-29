using Application.Features.Admin.Dtos;
using Application.Features.Admin.Interfaces;
using MediatR;

namespace Application.Features.Admin.Queries.GetAdminStats
{
    /// <summary>
    /// Handler pour les statistiques admin, comme un rapport de mission dans One Piece.
    /// </summary>
    public class GetAdminStatsHandler : IRequestHandler<GetAdminStatsQuery, AdminStatsDto>
    {
        private readonly IAdminRepository _adminRepository;

        public GetAdminStatsHandler(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<AdminStatsDto> Handle(GetAdminStatsQuery request, CancellationToken ct)
        {
            return await _adminRepository.GetGlobalStatsAsync();
        }
    }
}