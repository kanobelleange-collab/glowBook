using System;

namespace   Domain.Entities
{
    public class Avis
    {
        public Guid Id { get; private set; }
        public int Note { get; private set; }
        public string Commentaire { get; private set; }
        public DateTime DateAvis { get; private set; }
        public Guid ClientId { get; private set; }
        public Guid EtablissementId { get; private set; }
        public Guid RendezVousId { get; private set; }
        public bool EstVisible { get; private set; }
        public string? ReponseEtablissement { get; private set; }

        public Avis(Guid clientId, Guid etablissementId, Guid rendezVousId, int note, string commentaire)
        {
            if (note < 1 || note > 5)
                throw new ArgumentException("La note doit être entre 1 et 5.");

            Id              = Guid.NewGuid();
            ClientId        = clientId;
            EtablissementId = etablissementId;
            RendezVousId    = rendezVousId;
            Note            = note;
            Commentaire     = commentaire;
            DateAvis        = DateTime.UtcNow;
            EstVisible      = true;
        }

        public void Repondre(string reponse) => ReponseEtablissement = reponse;
        public void Masquer()  => EstVisible = false;
        public void Afficher() => EstVisible = true;

        public string EtoilesFormatees => new string('★', Note) + new string('☆', 5 - Note);
    }
}