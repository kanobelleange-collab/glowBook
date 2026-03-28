using System;
using Domain.Entities;

namespace Application.Features.Notifications.Interfaces
{
    // Interface pour l'envoi de notifications
    // (email ou SMS selon le choix du client)
    public interface INotificationService
    {
        // Notifier le client que son RDV est confirmé
        Task EnvoyerConfirmationRdvAsync(Guid clientId, Guid rendezVousId);

        // Rappel automatique 24h avant le RDV
        Task EnvoyerRappelRdvAsync(Guid clientId, Guid rendezVousId);

        // Notifier le client que son RDV est annulé
        Task EnvoyerAnnulationRdvAsync(Guid clientId, Guid rendezVousId, string raison);

        // Notifier le salon qu'un nouveau RDV a été réservé
        Task EnvoyerNouveauRdvSalonAsync(Guid etablissementId, Guid rendezVousId);
    }
}