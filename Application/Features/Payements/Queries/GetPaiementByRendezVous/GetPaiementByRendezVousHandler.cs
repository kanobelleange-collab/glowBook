using MediatR;
using Application.Features.Payements.DTOs;
using Application.Features.Payements.Interfaces;
using AutoMapper;
using Application.Features.Payements.Queries.GetPaiementByRendezVous;

namespace Application.Features.Payements.Queries.GetByRendezVous
{
    public class GetPaiementByRendezVousHandler : IRequestHandler<GetPaiementByRendezVousQuery, PaiementDto?>
    {
        private readonly IPaiementRepository _repository;
        private readonly IMapper _mapper;

        public GetPaiementByRendezVousHandler(IPaiementRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PaiementDto?> Handle(GetPaiementByRendezVousQuery request, CancellationToken cancellationToken)
        {
            var paiement = await _repository.GetByRendezVousIdAsync(request.RendezVousId);
            return _mapper.Map<PaiementDto>(paiement);
        }
    }
}