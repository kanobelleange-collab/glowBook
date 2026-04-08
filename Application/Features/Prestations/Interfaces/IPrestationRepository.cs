using Application.Features.Prestations.DTOs;
using Domain.Entities;

namespace Application.Features.Prestations.Interfaces
{
    public interface IPrestationRepository
    {
        Task<Prestation?> GetByIdAsync(Guid id);
        Task<List<PrestationDto>> GetByServiceAsync(Guid serviceId);

        
        Task<List<PrestationDto>> GetByEtablissementAsync(Guid etablissementId);
        

        Task<PrestationDto> AddAsync(Prestation prestation);
        Task<PrestationDto> UpdateAsync(Prestation prestation);
        
    }
}