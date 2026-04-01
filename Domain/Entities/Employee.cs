using System;

namespace   Domain.Entities
{
    public class Employee
    {
        public Guid Id { get;  set; }
        public Guid EtablissementId { get;  set; }
        public string Nom { get; set; }
        public string Prenom { get;  set; }
        public string Specialite { get; set; }
        public string? Photo { get; set; }
        
        public String  DateCreation{get;set;}

        public int AnneesExperience { get; set; }
        public double NoteMoyenne { get;  set; }
       
        public List<Disponibilite> Disponibilites { get;  set; }

        public Employee(Guid etablissementId,string nom, string prenom, string specialite,  String  dateCreation,int anneesExperience = 0)
        {
            Id               = Guid.NewGuid();
            EtablissementId  = etablissementId;
            Nom              = nom;
            Prenom           = prenom;
            Specialite       = specialite;
            DateCreation       =dateCreation;
        
            AnneesExperience = anneesExperience;
            Disponibilites   = new List<Disponibilite>();
            NoteMoyenne      = 0;
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