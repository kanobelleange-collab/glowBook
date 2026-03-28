using MediatR;
using Application.Features.ServiceEsthetiques.DTOs;
using Application.Features.ServiceEsthtiques.Interfaces;
using AutoMapper;

namespace Application.Features.ServiceEsthetiques.Queries.GetAll.GetSeviceByEtablissement
{

    public class GetServicesByEtablissementQueryHandler
        : IRequestHandler<GetServicesByEtablissementQuery, List<ServiceEsthetiqueDto>>
    {
        private readonly IServiceEsthetiqueRepository _serviceRepository;
        private readonly IMapper _mapper;

        public GetServicesByEtablissementQueryHandler(
            IServiceEsthetiqueRepository serviceRepository,
            IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper            = mapper;
        }

        public async Task<List<ServiceEsthetiqueDto>> Handle(
            GetServicesByEtablissementQuery query,
            CancellationToken cancellationToken)
        {
            var services = query.DisponiblesSeulement
                ? await _serviceRepository.GetDisponiblesAsync(query.EtablissementId)
                : await _serviceRepository.GetByEtablissementAsync(query.EtablissementId);

            return _mapper.Map<List<ServiceEsthetiqueDto>>(services);
        }
    }
}