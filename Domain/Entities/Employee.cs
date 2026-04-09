using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    public class Employee
    {
        public Guid Id { get; set; }
        public Guid EtablissementId { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Specialite { get; set; }
        public string? UrlPhoto { get; set; }
        public string DateHeure { get; set; }
        public int AnneeExperience { get; set; }
        
        // Initialiser la liste ici permet d'éviter des erreurs quand Dapper crée l'objet
        public List<Disponibilite> Disponibilites { get; set; } = new List<Disponibilite>();
    public void AjouterDisponibilite(Disponibilite d) 
    {
    d.EmployeeId = this.Id; // <--- C'est cette ligne qui lie la dispo à l'employé
    Disponibilites.Add(d);
    }
        // --- 1. LE CONSTRUCTEUR VIDE (OBLIGATOIRE POUR DAPPER) ---
        public Employee()
        {
            // Laissé vide volontairement. 
            // Dapper utilise ceci pour "matérialiser" l'employé depuis la base de données.
            DateHeure = string.Empty;
            Nom = string.Empty;
            Prenom = string.Empty;
            Specialite = string.Empty;
        }

        // --- 2. TON CONSTRUCTEUR EXISTANT ---
        public Employee(Guid etablissementId, string nom, string prenom, string specialite, string dateHeure, int anneeExperience = 0)
        {
            Id = Guid.NewGuid();
            EtablissementId = etablissementId;
            Nom = nom;
            Prenom = prenom;
            Specialite = specialite;
            DateHeure = dateHeure;
            AnneeExperience = anneeExperience;
            Disponibilites = new List<Disponibilite>();
        }

        

        public bool EstDisponible(DateTime dateHeure) =>
            Disponibilites.Any(d =>
                d.Jour == dateHeure.DayOfWeek &&
                d.HeureDebut <= dateHeure.TimeOfDay &&
                d.HeureFin >= dateHeure.TimeOfDay);

        public string NomComplet => $"{Prenom} {Nom}";
    }

    public class Disponibilite
    {
        public Guid EmployeeId { get; set; }
        public DayOfWeek Jour { get; set; } // Changé en 'set' pour Dapper
        public TimeSpan HeureDebut { get; set; } // Changé en 'set' pour Dapper
        public TimeSpan HeureFin { get; set; } // Changé en 'set' pour Dapper

        // --- CONSTRUCTEUR VIDE POUR DISPONIBILITE ---
        public Disponibilite() { }

        public Disponibilite(DayOfWeek jour, TimeSpan debut, TimeSpan fin)
        {
            if (fin <= debut)
                throw new ArgumentException("L'heure de fin doit être après l'heure de début.");
            Jour = jour;
            HeureDebut = debut;
            HeureFin = fin;
        }
    
       public static TimeSpan ParseHeure(string heure)
    {
    if (string.IsNullOrWhiteSpace(heure))
        return TimeSpan.Zero;

    // Gestion du format "8h00" ou "8h30"
    if (heure.Contains('h'))
    {
        var parts = heure.Split('h');
        int heures = int.Parse(parts[0]);
        int minutes = (parts.Length > 1 && !string.IsNullOrEmpty(parts[1]))
         ? int.Parse(parts[1])
        : 0;
        return new TimeSpan(heures, minutes, 0);
    }
    
    // Fallback au parsing standard
    return TimeSpan.Parse(heure);
    
    }
    public static DayOfWeek ConvertirJour(string jour)
    {
        return jour.Trim().ToLower() switch
        {
        "lundi" => DayOfWeek.Monday,
        "mardi" => DayOfWeek.Tuesday,
        "mercredi" => DayOfWeek.Wednesday,
        "jeudi" => DayOfWeek.Thursday,
        "vendredi" => DayOfWeek.Friday,
        "samedi" => DayOfWeek.Saturday,
        "dimanche" => DayOfWeek.Sunday,

        // Support anglais aussi (bonus)
        "monday" => DayOfWeek.Monday,
        "tuesday" => DayOfWeek.Tuesday,
        "wednesday" => DayOfWeek.Wednesday,
        "thursday" => DayOfWeek.Thursday,
        "friday" => DayOfWeek.Friday,
        "saturday" => DayOfWeek.Saturday,
        "sunday" => DayOfWeek.Sunday,

        _ => throw new Exception($"Jour invalide : {jour}")
        };

    }
}
}
