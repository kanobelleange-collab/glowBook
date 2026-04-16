
using Application.Features.Admin.Interfaces;
using MediatR;

namespace Application.Features.Admin.Commands.ResolveLitige
{
    public class ResolveLitigeHandler : IRequestHandler<ResolveLitigeCommand, bool>
    {
        private readonly IAdminRepository _adminRepository;
        public ResolveLitigeHandler(IAdminRepository adminRepository) => _adminRepository = adminRepository;

        public async Task<bool> Handle(ResolveLitigeCommand request, CancellationToken ct)
        {
            var success = await _adminRepository.UpdateLitigeStatusAsync(request.LitigeId, request.NouveauStatut);
            if (!success) throw new KeyNotFoundException("Litige introuvable.");
            return success;
        }
    }
}