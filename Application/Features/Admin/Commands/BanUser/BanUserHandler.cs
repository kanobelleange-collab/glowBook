using Application.Features.Admin.Commands;
using Application.Features.Admin.Interfaces;
using MediatR;

namespace Application.Features.Admin.Commands.BanUser
{
    /// <summary>
    /// Handler pour bannir/débannir un utilisateur, comme exiler un pirate dans One Piece.
    /// </summary>
    public class BanUserHandler : IRequestHandler<BanUserCommand, bool>
    {
        private readonly IAdminRepository _adminRepository;

        public BanUserHandler(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<bool> Handle(BanUserCommand request, CancellationToken ct)
        {
            var success = await _adminRepository.ToggleUserStatusAsync(request.UserId, request.IsActive);
            if (!success)
            {
                throw new KeyNotFoundException("Utilisateur non trouvé, comme un fantôme dans un serveur.");
            }
            return success;
        }
    }
}