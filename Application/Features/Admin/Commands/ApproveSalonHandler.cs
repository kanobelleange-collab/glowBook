using Application.Features.Admin.Commands;
using Application.Features.Admin.Interfaces;
using MediatR;

namespace Application.Features.Admin.Commands
{
    /// <summary>
    /// Handler pour approuver un salon, comme accorder une licence de pirate dans One Piece.
    /// </summary>
    public class ApproveSalonHandler : IRequestHandler<ApproveSalonCommand, bool>
    {
        private readonly IAdminRepository _adminRepository;

        public ApproveSalonHandler(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<bool> Handle(ApproveSalonCommand request, CancellationToken ct)
        {
            var success = await _adminRepository.ApproveEtablissementAsync(request.SalonId);
            if (!success)
            {
                throw new KeyNotFoundException("Salon non trouvé, comme un pirate disparu.");
            }
            return success;
        }
    }
}