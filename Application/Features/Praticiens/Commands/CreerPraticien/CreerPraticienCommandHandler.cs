using AutoMapper;
using MediatR;
using Application.Features.Praticiens.DTOs;
using Application.Features.Praticiens.Interfaces;
using Domain.Entities;

namespace Application.Features.Praticiens.Commands
{
    public class CreerPraticienCommandHandler
        : IRequestHandler<CreerPraticienCommand, PraticienDto>
    {
        private readonly IPraticienRepository _praticienRepository;
        private readonly IMapper _mapper;

        public CreerPraticienCommandHandler(
            IPraticienRepository praticienRepository,
            IMapper mapper)
        {
            _praticienRepository = praticienRepository;
            _mapper              = mapper;
        }

        public async Task<PraticienDto> Handle(
            CreerPraticienCommand command,
            CancellationToken cancellationToken)
        {
            // 1. Créer l'entité
            var praticien = new Praticien(
                command.Nom,
                command.Prenom,
                command.Specialite,
                command.EtablissementId,
                command.AnneesExperience
            );

          // 2. Ajouter la description si présente
if (!string.IsNullOrWhiteSpace(command.Description))
    praticien.MettreAJourDescription(command.Description);
            // 3. Sauvegarder en base
            await _praticienRepository.AjouterAsync(praticien);

            // 4. Mapper et retourner
            return _mapper.Map<PraticienDto>(praticien);
        }
    }
}