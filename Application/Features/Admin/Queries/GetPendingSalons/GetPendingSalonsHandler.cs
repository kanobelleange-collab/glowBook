using Application.Features.Admin.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Features.Admin.Queries.GetPendingSalons
{
    /// <summary>
    /// Handler pour les salons en attente, comme surveiller les nouveaux pirates dans One Piece.
    /// </summary>
    public class GetPendingSalonsHandler : IRequestHandler<GetPendingSalonsQuery, List<Etablissement>>
    {
        private readonly IAdminRepository _adminRepository;

        public GetPendingSalonsHandler(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<List<Etablissement>> Handle(GetPendingSalonsQuery request, CancellationToken ct)
        {
            return await _adminRepository.GetPendingEtablissementsAsync();
        }
    }
}