using System;

namespace   Domain.Entities
{
    public class Praticien
    {
        public Guid Id { get; private set; }
        public string Nom { get; private set; }
        public string Prenom { get; private set; }
        public string Specialite { get; private set; }
        public string? Photo { get; private set; }
        public string? Description { get; private set; }
        public int AnneesExperience { get; private set; }
        public double NoteMoyenne { get; private set; }
        public Guid EtablissementId { get; private set; }
        public List<Disponibilite> Disponibilites { get; private set; }

        public Praticien(string nom, string prenom, string specialite, Guid etablissementId, int anneesExperience = 0)
        {
            Id               = Guid.NewGuid();
            Nom              = nom;
            Prenom           = prenom;
            Specialite       = specialite;
            EtablissementId  = etablissementId;
            AnneesExperience = anneesExperience;
            Disponibilites   = new List<Disponibilite>();
            NoteMoyenne      = 0;
        }
        // ✅ Méthode dans la bonne classe
        public void MettreAJourDescription(string description)
        {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("La description ne peut pas être vide.");
        Description = description;
        }


        public void AjouterDisponibilite(Disponibilite d) => Disponibilites.Add(d);

        public bool EstDisponible(DateTime dateHeure) =>
            Disponibilites.Any(d =>
                d.Jour       == dateHeure.DayOfWeek &&
                d.HeureDebut <= dateHeure.TimeOfDay &&
                d.HeureFin   >= dateHeure.TimeOfDay);

        public string NomComplet => $"{Prenom} {Nom}";
    }
    

    public class Disponibilite
    {
        public DayOfWeek Jour { get; private set; }
        public TimeSpan HeureDebut { get; private set; }
        public TimeSpan HeureFin { get; private set; }

        public Disponibilite(DayOfWeek jour, TimeSpan debut, TimeSpan fin)
        {
            if (fin <= debut)
                throw new ArgumentException("L'heure de fin doit être après l'heure de début.");
            Jour       = jour;
            HeureDebut = debut;
            HeureFin   = fin;
        }
        
    }
    

}