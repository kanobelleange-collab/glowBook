using Application.Features.Notifications.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(ILogger<NotificationService> logger)
        {
            _logger = logger;
        }

        public async Task EnvoyerConfirmationRdvAsync(Guid clientId, Guid rendezVousId)
        {
            // Simulation : Ici plus tard, tu appelleras une API de SMS (comme Orange/MTN) ou un SMTP Email
            _logger.LogInformation($"[NOTIFICATION] Confirmation envoyée au Client {clientId} pour le RDV {rendezVousId}.");
            await Task.CompletedTask;
        }

        public async Task EnvoyerRappelRdvAsync(Guid clientId, Guid rendezVousId)
        {
            _logger.LogInformation($"[NOTIFICATION - RAPPEL 24H] Rappel envoyé au Client {clientId} pour le RDV {rendezVousId}.");
            await Task.CompletedTask;
        }

        public async Task EnvoyerAnnulationRdvAsync(Guid clientId, Guid rendezVousId, string raison)
        {
            _logger.LogWarning($"[NOTIFICATION - ANNULATION] Client {clientId}, votre RDV {rendezVousId} est annulé. Raison : {raison}");
            await Task.CompletedTask;
        }

        public async Task EnvoyerNouveauRdvSalonAsync(Guid etablissementId, Guid rendezVousId)
        {
            _logger.LogInformation($"[NOTIFICATION - SALON] L'établissement {etablissementId} a reçu une nouvelle réservation : {rendezVousId}.");
            await Task.CompletedTask;
        }
    }
}