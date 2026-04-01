using System;

namespace Domain.Entities
{
    public class Etablissement
    {
        public Guid Id { get; private set; }
        public string Nom { get; private set; }
        public string Adresse { get; private set; }
        public string Quartier { get; private set; }
        public string Ville { get; private set; }
        public string Telephone { get; private set; }
        public string Email { get; private set; }
        public string? Description { get; private set; }
        public double Note { get; private set; }
        public double NoteMoyenne { get; private set; }
        public bool EstActif { get; private set; }
        public DateTime DateCreation { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public List<string> Photos { get; private set; }
        public List<HoraireOuverture> Horaires { get; private set; }
        public List<EtablissementService> Services { get; private set; }

        public Etablissement(
            string nom,
            string adresse,
            string ville,
            string telephone,
            string email,
            string? description = null)
        {
            Id           = Guid.NewGuid();
            Nom          = nom;
            Adresse      = adresse;
            Quartier     = string.Empty;
            Ville        = ville;
            Telephone    = telephone;
            Email        = email;
            Description  = description;
            Note         = 0;
            NoteMoyenne  = 0;
            EstActif     = true;
            DateCreation = DateTime.UtcNow;
            Photos       = new List<string>();
            Horaires     = new List<HoraireOuverture>();
            Services     = new List<EtablissementService>();
        }

        // Constructeur vide pour Dapper
        public Etablissement()
        {
            Nom       = string.Empty;
            Adresse   = string.Empty;
            Quartier  = string.Empty;
            Ville     = string.Empty;
            Telephone = string.Empty;
            Email     = string.Empty;
            Photos    = new List<string>();
            Horaires  = new List<HoraireOuverture>();
            Services  = new List<EtablissementService>();
        }

        public void DefinirQuartier(string quartier) => Quartier = quartier;

        public void DefinirLocalisation(double latitude, double longitude)
        {
            Latitude  = latitude;
            Longitude = longitude;
        }

        public void MettreAJour(
            string nom, string adresse,string quartier, string ville,
            string telephone, string email, string? description = null)
        {
            Nom         = nom;
            Adresse     = adresse;
            Quartier    =quartier;
            Ville       = ville;
            Telephone   = telephone;
            Email       = email;
            Description = description;
        }

        public void MettreAJourNote(double note) => Note = note;
        public void MettreAJourNoteMoyenne(double nouvelleNote) => NoteMoyenne = nouvelleNote;
        public void Desactiver() => EstActif = false;

        public void AjouterPhoto(string urlPhoto)
        {
            if (!string.IsNullOrWhiteSpace(urlPhoto))
                Photos.Add(urlPhoto);
        }

        public void AjouterHoraire(HoraireOuverture horaire) =>
            Horaires.Add(horaire);

        public void AjouterService(EtablissementService service)
        {
            if (!Services.Any(s => s.TypeServiceNom == service.TypeServiceNom))
                Services.Add(service);
        }
    }

    // -------------------------------------------------------
    // Service proposé par l'établissement (ex: Coiffure, Massage)
    // -------------------------------------------------------
    // -------------------------------------------------------
    // Prestation avec son propre prix et durée
    // ex: "Box Braid 50€ 3h", "Locks 80€ 4h"
    // -------------------------------------------------------

    // -------------------------------------------------------
    // Horaires d'ouverture
    // -------------------------------------------------------
    public class HoraireOuverture
    {
        public DayOfWeek Jour { get; private set; }
        public TimeSpan HeureOuverture { get; private set; }
        public TimeSpan HeureFermeture { get; private set; }
        public bool EstFerme { get; private set; }

        public HoraireOuverture(
            DayOfWeek jour, TimeSpan ouverture,
            TimeSpan fermeture, bool estFerme = false)
        {
            Jour           = jour;
            HeureOuverture = ouverture;
            HeureFermeture = fermeture;
            EstFerme       = estFerme;
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