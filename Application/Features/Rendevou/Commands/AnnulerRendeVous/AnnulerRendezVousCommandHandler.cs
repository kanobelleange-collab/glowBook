using System;
using MediatR;
using Application.Features.Rendevou.Interfaces;
using Application.Features.Notifications.Interfaces;
using Domain.Entities;

namespace Application.Features.Rendevou.Commands.AnnulerRendeVous
{
    public class AnnulerRendeVousCommandHandler
        : IRequestHandler<AnnulerRendeVousCommand>
    {
        private readonly IRendezVousRepository _rdvRepository;
        private readonly INotificationService _notificationService;

        public AnnulerRendeVousCommandHandler(
            IRendezVousRepository rdvRepository,
            INotificationService notificationService)
        {
            _rdvRepository       = rdvRepository;
            _notificationService = notificationService;
        }

        public async Task Handle(
            AnnulerRendeVousCommand command,
            CancellationToken cancellationToken)
        {
            // 1. Récupérer le RDV
            var rdv = await _rdvRepository.GetByIdAsync(command.RendezVousId)
                      ?? throw new Exception("Rendez-vous introuvable.");
            // ✅ rdv est maintenant RendezVous (non-nullable) grâce à ??  throw

            // 2. Vérifier que le client est bien le propriétaire
            if (rdv.ClientId != command.ClientId)
                throw new Exception("Vous ne pouvez pas annuler le RDV d'un autre client.");

            // 3. Vérifier que l'annulation est encore possible
            if (!rdv.PeutEtreAnnule())
                throw new Exception("Annulation impossible moins de 24h avant le RDV.");

            // 4. Appliquer l'annulation
            var raison = command.Raison ?? "Aucune raison fournie";
            rdv.Annuler(raison);
            await _rdvRepository.MettreAJourAsync(rdv);

            // 5. Envoyer la notification
            await _notificationService
                .EnvoyerAnnulationRdvAsync(command.ClientId, rdv.Id, raison);
        }
    }
}