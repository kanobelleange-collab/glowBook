using MediatR;
using Application.Features.Rendevou.Interfaces;
using Application.Features.Rendevou.DTOs;


namespace Application.Features.Rendevou.Queries.GetAll
{
    // Retourne une liste de RendezVousDto
   

    public class GetRendezVousClientQueryHandler
        : IRequestHandler<GetRendezVousClientQuery, List<RendezVousDto>>
    {
        private readonly IRendezVousRepository _rdvRepository;

        public GetRendezVousClientQueryHandler(IRendezVousRepository rdvRepository)
        {
            _rdvRepository = rdvRepository;
        }

        public async Task<List<RendezVousDto>> Handle(
            GetRendezVousClientQuery query,
            CancellationToken cancellationToken)
        {
            var rdvs = await _rdvRepository.GetByClientAsync(query.ClientId);

            if (query.Statut.HasValue)
                rdvs = rdvs.Where(r => r.Statut == query.Statut.Value).ToList();

            return rdvs.Select(r => new RendezVousDto
            {
                Id              = r.Id,
                DateHeure       = r.DateHeure,
                Statut          = r.Statut,
                Prix            = r.Prix,
                PraticienId     = r.PraticienId,
                ServiceId       = r.ServiceId,
                EtablissementId = r.EtablissementId
            }).ToList();
        }
    }
}