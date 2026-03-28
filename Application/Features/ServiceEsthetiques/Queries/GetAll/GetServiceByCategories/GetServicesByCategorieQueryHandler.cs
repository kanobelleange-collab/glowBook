using MediatR;
using Application.Features.ServiceEsthetiques.DTOs;
using Application.Features.ServiceEsthtiques.Interfaces;
using AutoMapper;

namespace Application.Features.ServiceEsthetiques.Queries.GetAll.GetServiceByCategories
{
  

    public class GetServicesByCategorieQueryHandler
        : IRequestHandler<GetServicesByCategorieQuery, List<ServiceEsthetiqueDto>>
    {
        private readonly IServiceEsthetiqueRepository _serviceRepository;
        private readonly IMapper _mapper;

        public GetServicesByCategorieQueryHandler(
            IServiceEsthetiqueRepository serviceRepository,
            IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper            = mapper;
        }

        public async Task<List<ServiceEsthetiqueDto>> Handle(
            GetServicesByCategorieQuery query,
            CancellationToken cancellationToken)
        {
            var services = await _serviceRepository
                .GetByCategorieAsync(query.Categorie);

            return _mapper.Map<List<ServiceEsthetiqueDto>>(services);
        }
    }
}