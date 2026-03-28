using System;

namespace Domain.Entities
{
    public class SalonMassage : Etablissement
    {
        public List<string> TypesMassage { get; private set; }
        public string Ambiance { get; private set; }
        public bool DisponibleADomicile { get; private set; }
        public int DureeMinimaleEnMinutes { get; private set; }

        public SalonMassage(
            string nom, string adresse, string ville,
            string telephone, string email,
            List<string> typesMassage,
            string ambiance,
            bool disponibleADomicile   = false,
            int dureeMinimaleEnMinutes = 30,
            string? description        = null)  // ✅ optionnel en dernier
            : base(nom, adresse, ville, telephone, email,
                   CategorieEtablissement.SalonMassage, // ✅ catégorie fixe
                   description)
        {
            TypesMassage           = typesMassage ?? new List<string>();
            Ambiance               = ambiance;
            DisponibleADomicile    = disponibleADomicile;
            DureeMinimaleEnMinutes = dureeMinimaleEnMinutes;
        }

        public void AjouterTypeMassage(string t) => TypesMassage.Add(t);
        public void ActiverServiceADomicile() => DisponibleADomicile = true;
    }
}