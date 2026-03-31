using MediatR;
using AutoMapper;
using Application.Features.Rendevou.Interfaces;
using Application.Features.Rendevou.DTOs;

namespace Application.Features.Rendevou.Queries.GetByClient
{
    public class GetRendezVousByClientHandler : IRequestHandler<GetRendezVousByClientQuery, List<RendezVousDto>>
    {
        private readonly IRendezVousRepository _repository;
        private readonly IMapper _mapper;

        public GetRendezVousByClientHandler(IRendezVousRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<RendezVousDto>> Handle(GetRendezVousByClientQuery request, CancellationToken ct)
        {
            var data = await _repository.GetByClientAsync(request.ClientId);
            return _mapper.Map<List<RendezVousDto>>(data);
        }
    }
}