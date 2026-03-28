using System;

namespace Domain.Entities
{
    public class CabinetProthetiste : Etablissement
    {
        public bool ProposeProtheseOngles { get; private set; }
        public bool ProposeExtensionCils { get; private set; }
        public bool ProposeProtheseCapillaire { get; private set; }
        public List<string> MatieresOngles { get; private set; }
        public List<string> StylesCils { get; private set; }

        public CabinetProthetiste(
            string nom, string adresse, string ville,
            string telephone, string email,
            bool prothOngles     = true,
            bool extCils         = false,
            bool prothCapillaire = false,
            string? description  = null)  // ✅ optionnel en dernier
            : base(nom, adresse, ville, telephone, email,
                   CategorieEtablissement.SalonOnglerie, // ✅ catégorie fixe
                   description)
        {
            ProposeProtheseOngles     = prothOngles;
            ProposeExtensionCils      = extCils;
            ProposeProtheseCapillaire = prothCapillaire;
            MatieresOngles            = new List<string>();
            StylesCils                = new List<string>();
        }

        public void AjouterMatiereOngles(string m) => MatieresOngles.Add(m);
        public void AjouterStyleCils(string s) => StylesCils.Add(s);
    }
}