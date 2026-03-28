using System;

namespace   Domain.Entities
{
    public class ServiceEsthetique
    {
        public Guid Id { get; private set; }
        public string Nom { get; private set; }
        public string Description { get; private set; }
        public decimal Prix { get; private set; }
        public int DureeEnMinutes { get; private set; }
        public string Categorie { get; private set; }
        public Guid EtablissementId { get; private set; }
        public bool EstDisponible { get; private set; }

        public ServiceEsthetique(
            string nom, string description, decimal prix,
            int dureeEnMinutes, string categorie, Guid etablissementId)
        {
            if (prix < 0) throw new ArgumentException("Le prix ne peut pas être négatif.");
            if (dureeEnMinutes <= 0) throw new ArgumentException("La durée doit être > 0.");

            Id              = Guid.NewGuid();
            Nom             = nom;
            Description     = description;
            Prix            = prix;
            DureeEnMinutes  = dureeEnMinutes;
            Categorie       = categorie;
            EtablissementId = etablissementId;
            EstDisponible   = true;
        }

        public void ModifierPrix(decimal p)
        {
            if (p < 0) throw new ArgumentException("Prix invalide.");
            Prix = p;
        }

        public void Desactiver() => EstDisponible = false;
        public void Activer()    => EstDisponible = true;

        public string DureeFormatee =>
            DureeEnMinutes >= 60
                ? $"{DureeEnMinutes / 60}h {DureeEnMinutes % 60}min"
                : $"{DureeEnMinutes}min";
    }
}