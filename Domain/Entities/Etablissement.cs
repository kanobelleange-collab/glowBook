using System;

namespace Domain.Entities
{
    public class Etablissement  // ✅ abstract supprimé
    {
        public Guid Id { get; private set; }
        public string Nom { get; private set; }
        public string Adresse { get; private set; }
        public string Ville { get; private set; }
        public string Telephone { get; private set; }
        public string Email { get; private set; }
      public CategorieEtablissement Categorie { get; private set; } // ✅ plus sûr qu'un string    // ✅ ajouté
        public string? Description { get; private set; }  // ✅ ajouté
        public double Note { get; private set; }           // ✅ ajouté
        public List<string> Photos { get; private set; }
        public List<HoraireOuverture> Horaires { get; private set; }
        public double NoteMoyenne { get; private set; }
        public bool EstActif { get; private set; }
        public DateTime DateCreation { get; private set; }

        public Etablissement(
            string nom,
            string adresse,
            string ville,
            string telephone,
            string email,
        CategorieEtablissement  categorie , 
            string? description = null)  // ✅ ajouté
        {
            Id           = Guid.NewGuid();
            Nom          = nom;
            Adresse      = adresse;
            Ville        = ville;
            Telephone    = telephone;
            Email        = email;
            Categorie    = categorie;
            Description  = description;
            Note         = 0;
            Photos       = new List<string>();
            Horaires     = new List<HoraireOuverture>();
            NoteMoyenne  = 0;
            EstActif     = true;
            DateCreation = DateTime.UtcNow;
        }

        // ✅ Constructeur vide pour Dapper
        public Etablissement() 
        {
            Nom      = string.Empty;
            Adresse  = string.Empty;
            Ville    = string.Empty;
            Telephone = string.Empty;
            Email    = string.Empty;
            Photos   = new List<string>();
            Horaires = new List<HoraireOuverture>();
        }

        public void MettreAJour(string nom, string adresse, string ville, 
            string telephone, string email,     CategorieEtablissement categorie , string? description = null)
        {
            Nom         = nom;
            Adresse     = adresse;
            Ville       = ville;
            Telephone   = telephone;
            Email       = email;
            Categorie   = categorie;
            Description = description;
        }

        public void MettreAJourNote(double note)  // ✅ pour Note
        {
            Note = note;
        }

        public void MettreAJourNoteMoyenne(double nouvelleNote)
        {
            NoteMoyenne = nouvelleNote;
        }

        public void Desactiver() => EstActif = false;

        public void AjouterPhoto(string urlPhoto)
        {
            if (!string.IsNullOrWhiteSpace(urlPhoto))
                Photos.Add(urlPhoto);
        }

        public void AjouterHoraire(HoraireOuverture horaire)
        {
            Horaires.Add(horaire);
        }
    }

    public class HoraireOuverture
    {
        public DayOfWeek Jour { get; private set; }
        public TimeSpan HeureOuverture { get; private set; }
        public TimeSpan HeureFermeture { get; private set; }
        public bool EstFerme { get; private set; }

        public HoraireOuverture(DayOfWeek jour, TimeSpan ouverture, TimeSpan fermeture, bool estFerme = false)
        {
            Jour           = jour;
            HeureOuverture = ouverture;
            HeureFermeture = fermeture;
            EstFerme       = estFerme;
        }

        public static HoraireOuverture JourFerme(DayOfWeek jour)
            => new HoraireOuverture(jour, TimeSpan.Zero, TimeSpan.Zero, estFerme: true);
    }
}