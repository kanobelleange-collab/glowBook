using System;


namespace Domain.Entities
{
    public class RendezVous
    {
        public Guid Id { get; private set; }
        public DateTime DateHeure { get; private set; }
        public StatutRendezVous Statut { get; private set; }
        public decimal Prix { get; private set; }
        public Guid ClientId { get; private set; }
        public Guid PraticienId { get; private set; }
        public Guid ServiceId { get; private set; }
        public Guid EtablissementId { get; private set; }
        public string? NotesClient { get; private set; }
        public string? RaisonAnnulation { get; private set; }
        public DateTime DateCreation { get; private set; }

        public RendezVous(
            Guid clientId, Guid praticienId, Guid serviceId,
            Guid etablissementId, DateTime dateHeure, decimal prix,
            string? notesClient = null)
        {
            Id              = Guid.NewGuid();
            ClientId        = clientId;
            PraticienId     = praticienId;
            ServiceId       = serviceId;
            EtablissementId = etablissementId;
            DateHeure       = dateHeure;
            Prix            = prix;
            NotesClient     = notesClient;
            Statut          = StatutRendezVous.EnAttente;
            DateCreation    = DateTime.UtcNow;
        }

        public void Confirmer()
        {
            if (Statut != StatutRendezVous.EnAttente)
                throw new InvalidOperationException("Seul un RDV en attente peut être confirmé.");
            Statut = StatutRendezVous.Confirme;
        }

        public void Annuler(string raison)
        {
            if (Statut == StatutRendezVous.Termine)
                throw new InvalidOperationException("Impossible d'annuler un RDV terminé.");
            Statut           = StatutRendezVous.Annule;
            RaisonAnnulation = raison;
        }

        public void Terminer()
        {
            if (Statut != StatutRendezVous.Confirme)
                throw new InvalidOperationException("Seul un RDV confirmé peut être terminé.");
            Statut = StatutRendezVous.Termine;
        }

        public bool PeutEtreAnnule(int heuresAvant = 24) =>
            Statut != StatutRendezVous.Termine &&
            Statut != StatutRendezVous.Annule  &&
            DateHeure > DateTime.UtcNow.AddHours(heuresAvant);
    }

    public enum StatutRendezVous
    {
        EnAttente,
        Confirme,
        Annule,
        Termine
    }
}