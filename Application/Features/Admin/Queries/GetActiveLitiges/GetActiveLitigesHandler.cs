using Application.Features.Admin.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Features.Admin.Queries
{
    public class GetActiveLitigesHandler : IRequestHandler<GetActiveLitigesQuery, List<Litige>>
    {
        private readonly IAdminRepository _adminRepository;

        public GetActiveLitigesHandler(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<List<Litige>> Handle(GetActiveLitigesQuery request, CancellationToken ct)
        {
            // Appelle la méthode que nous avons ajoutée à l'IAdminRepository
            return await _adminRepository.GetActiveLitigesAsync();
        }
    }
}