using MediatR;
using Application.Features.Payements.DTOs;
using Application.Features.Payements.Interfaces;
using AutoMapper;
using Application.Features.Payements.Queries.GetHistoriqueClient;
namespace Application.Features.Payements.Queries.GetByClient
{

    public class GetPaiementsByClientHandler : IRequestHandler<GetPaiementsByClientQuery, List<PaiementDto>>
    {
        private readonly IPaiementRepository _repository;
        private readonly IMapper _mapper;

        public GetPaiementsByClientHandler(IPaiementRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PaiementDto>> Handle(GetPaiementsByClientQuery request, CancellationToken cancellationToken)
        {
            var paiements = await _repository.GetByClientAsync(request.ClientId);
            return _mapper.Map<List<PaiementDto>>(paiements);
        }
    }
}