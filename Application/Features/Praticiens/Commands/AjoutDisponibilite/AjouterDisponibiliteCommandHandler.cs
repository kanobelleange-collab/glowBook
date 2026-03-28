using AutoMapper;
using MediatR;
using Application.Features.Praticiens.DTOs;
using Application.Features.Praticiens.Interfaces;
using Domain.Entities;
using Application.Features.Praticiens.Commands.AjoutDisponibilite;

namespace Application.Features.Praticiens.Commands
{
    public class AjouterDisponibiliteCommandHandler
        : IRequestHandler<AjouterDisponibiliteCommand, PraticienDto>
    {
        private readonly IPraticienRepository _praticienRepository;
        private readonly IMapper _mapper;

        public AjouterDisponibiliteCommandHandler(
            IPraticienRepository praticienRepository,
            IMapper mapper)
        {
            _praticienRepository = praticienRepository;
            _mapper              = mapper;
        }

        public async Task<PraticienDto> Handle(
            AjouterDisponibiliteCommand command,
            CancellationToken cancellationToken)
        {
            // 1. Récupérer le praticien
            var praticien = await _praticienRepository.GetByIdAsync(command.PraticienId);
            if (praticien == null)
                throw new Exception($"Praticien avec l'Id {command.PraticienId} introuvable.");

            // 2. Convertir les strings en types appropriés
            var jour      = Enum.Parse<DayOfWeek>(command.Jour, ignoreCase: true);
            var heureDebut = TimeSpan.Parse(command.HeureDebut);
            var heureFin   = TimeSpan.Parse(command.HeureFin);

            // 3. Créer et ajouter la disponibilité
            var disponibilite = new Disponibilite(jour, heureDebut, heureFin);
            praticien.AjouterDisponibilite(disponibilite);

            // 4. Sauvegarder
            await _praticienRepository.MettreAJourAsync(praticien);

            // 5. Retourner le DTO mis à jour
            return _mapper.Map<PraticienDto>(praticien);
        }
    }
}