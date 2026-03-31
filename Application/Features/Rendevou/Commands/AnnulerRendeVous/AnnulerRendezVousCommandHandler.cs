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
            _rdvRepository = rdvRepository;
            _notificationService = notificationService;
        }

        public async Task Handle(
            AnnulerRendeVousCommand command,
            CancellationToken cancellationToken)
        {
            // 1. Récupérer le RDV via l'interface
            // Utilise RendezVousId ou Id selon le nom dans ta Record Command
            var rdv = await _rdvRepository.GetByIdAsync(command.RendezVousId)
                      ?? throw new Exception("Rendez-vous introuvable.");

            // 2. Vérifier la propriété (Sécurité : un client ne peut annuler que le sien)
            if (rdv.ClientId != command.ClientId)
                throw new Exception("Vous ne pouvez pas annuler le RDV d'un autre client.");

            // 3. Vérifier la règle métier des 24h définie dans l'entité Domaine
            // On utilise la méthode 'PeutEtreAnnule' que tu as créée dans la classe RendezVous
            if (!rdv.PeutEtreAnnule(24)) 
                throw new Exception("Annulation impossible moins de 24h avant le RDV.");

            // 4. Appliquer l'annulation
            // On s'assure que la raison n'est pas vide pour l'entité
            var raison = string.IsNullOrWhiteSpace(command.Raison) 
                         ? "Aucune raison fournie" 
                         : command.Raison;

            // Appel de la méthode métier de l'entité (change le statut et stocke la raison)
            rdv.Annuler(raison);

            // 5. Persistance via l'interface (Mise à jour dans la DB via Dapper)
            await _rdvRepository.MettreAJourAsync(rdv);

            // 6. Notification (Informer le praticien ou l'établissement)
            await _notificationService
                .EnvoyerAnnulationRdvAsync(command.ClientId, rdv.Id, raison);
        }
    }
}