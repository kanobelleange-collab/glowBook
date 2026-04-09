using Application.Features.Admin.Interfaces;
using Application.Features.Admin.Queries;
using MediatR;

namespace Application.Features.Admin.Queries
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