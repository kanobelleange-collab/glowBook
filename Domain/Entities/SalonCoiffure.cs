using System;

namespace Domain.Entities
{
    public class SalonCoiffure : Etablissement
    {
        public List<string> SpecialitesTresse { get; private set; }
        public List<string> TypesCheveux { get; private set; }
        public bool AccepteHommes { get; private set; }
        public bool AccepteEnfants { get; private set; }

        public SalonCoiffure(
            string nom,
            string adresse,
            string ville,
            string telephone,
            string email,
            List<string> specialitesTresse,  // ✅ obligatoire — remonté avant les optionnels
            List<string> typesCheveux,        // ✅ obligatoire — remonté avant les optionnels
            bool accepteHommes  = false,
            bool accepteEnfants = false,
            string? description = null)       // ✅ optionnel — mis en dernier
            : base(nom, adresse, ville, telephone, email,
                   CategorieEtablissement.SalonCoiffure,
                   description)
        {
            SpecialitesTresse = specialitesTresse ?? new List<string>();
            TypesCheveux      = typesCheveux ?? new List<string>();
            AccepteHommes     = accepteHommes;
            AccepteEnfants    = accepteEnfants;
        }

        public void AjouterSpecialite(string s) => SpecialitesTresse.Add(s);
        public void AjouterTypeCheveux(string t) => TypesCheveux.Add(t);
    }
}