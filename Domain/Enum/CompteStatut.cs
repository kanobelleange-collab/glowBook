namespace Domain.Enum
{
    public enum CompteStatut
    {
        EnAttente,     // Inscription faite, attend le "OK" de l'admin
        Actif,         // Visible sur la carte et réservable
        Suspendu,      // En cas de litige
        Rejete         // Inscription non conforme
    }
}