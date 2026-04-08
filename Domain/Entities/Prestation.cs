 using System;

 namespace Domain.Entities
{
        public class Prestation
    {
        public Guid Id { get;  set; }
        public Guid ServiceId { get; set; }
        public string Nom { get; set; }
        public string? Description { get; set; }
        public decimal Prix { get;  set; }
        public int DureeMinutes { get;  set; }

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