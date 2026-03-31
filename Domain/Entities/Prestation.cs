 using System;

 namespace Domain.Entities
{
        public class Prestation
    {
        public Guid Id { get; private set; }
        public Guid ServiceId { get; private set; }
        public string Nom { get; private set; }
        public string? Description { get; private set; }
        public decimal Prix { get; private set; }
        public int DureeMinutes { get; private set; }

        public Prestation(
            Guid serviceId,
            string nom,
            decimal prix,
            int dureeMinutes,
            string? description = null)
        {
            Id           = Guid.NewGuid();
            ServiceId    = serviceId;
            Nom          = nom;
            Prix         = prix;
            DureeMinutes = dureeMinutes;
            Description  = description;
        }

        public Prestation() { Nom = string.Empty; }

        public void MettreAJourPrix(decimal nouveauPrix) => Prix = nouveauPrix;
        public void MettreAJourDuree(int dureeMinutes) => DureeMinutes = dureeMinutes;
    }

}