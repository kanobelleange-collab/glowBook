using System;
using Domain.Enum;

namespace Domain.Entities
{
    public class SpaBeaute : Etablissement
    {
        public ServiceSpa Service { get; private set; }
        public List<string> SoinsCorps { get; private set; }
        public List<string> Rituels { get; private set; }
        public int NombresCabinesPrivees { get; private set; }

        public SpaBeaute(
            string nom, string adresse, string ville,
            string telephone, string email,
            ServiceSpa service  = ServiceSpa.None,
            int cabines         = 0,
            string? description = null)  // ✅ optionnel en dernier
            : base(nom, adresse, ville, telephone, email,
                   CategorieEtablissement.SpaBeaute, // ✅ catégorie fixe
                   description)
        {
            Service               = service;
            NombresCabinesPrivees = cabines;
            SoinsCorps            = new List<string>();
            Rituels               = new List<string>();
        }

        public void AjouterSoinCorps(string s) => SoinsCorps.Add(s);
        public void AjouterRituel(string r) => Rituels.Add(r);
    }
}