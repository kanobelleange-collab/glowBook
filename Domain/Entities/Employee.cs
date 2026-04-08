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
        public int AnneesExperience { get; set; }
        
        // Initialiser la liste ici permet d'éviter des erreurs quand Dapper crée l'objet
        public List<Disponibilite> Disponibilites { get; set; } = new List<Disponibilite>();

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
        public Employee(Guid etablissementId, string nom, string prenom, string specialite, string dateHeure, int anneesExperience = 0)
        {
            Id = Guid.NewGuid();
            EtablissementId = etablissementId;
            Nom = nom;
            Prenom = prenom;
            Specialite = specialite;
            DateHeure = dateHeure;
            AnneesExperience = anneesExperience;
            Disponibilites = new List<Disponibilite>();
        }

        public void AjouterDisponibilite(Disponibilite d) => Disponibilites.Add(d);

        public bool EstDisponible(DateTime dateHeure) =>
            Disponibilites.Any(d =>
                d.Jour == dateHeure.DayOfWeek &&
                d.HeureDebut <= dateHeure.TimeOfDay &&
                d.HeureFin >= dateHeure.TimeOfDay);

        public string NomComplet => $"{Prenom} {Nom}";
    }

    public class Disponibilite
    {
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
    }
}