using MediatR;
using Application.Features.Etablissements.DTOs;
using Application.Features.Etablissements.Interfaces;
using AutoMapper;

namespace Application.Features.Etablissements.Commands.UpdateEtablissement
{
    public class UpdateEtablissementHandler
        : IRequestHandler<UpdateEtablissementCommand, EtablissementDto>
    {
        private readonly IEtablissementRepository _repository;
        private readonly IMapper _mapper;

        public UpdateEtablissementHandler(
            IEtablissementRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper     = mapper;
        }

        public async Task<EtablissementDto> Handle(
            UpdateEtablissementCommand request,
            CancellationToken cancellationToken)
        {
            var etablissement = await _repository.GetByIdAsync(request.Id)
                ?? throw new Exception(
                    $"Établissement avec ID {request.Id} non trouvé.");

            etablissement.MettreAJour(
                request.Nom,
                request.Adresse,
                request.Quartier,
                request.Ville,
                request.Telephone,
                request.Email,
                request.Description);

            await _repository.UpdateAsync(etablissement);

            return _mapper.Map<EtablissementDto>(etablissement);
        }
    }
}