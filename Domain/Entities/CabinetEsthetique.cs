using System;

namespace Domain.Entities
{
    public class CabinetEsthetique : Etablissement
    {
        public bool ProposeSoinsVisage { get; private set; }
        public bool ProposeEpilation { get; private set; }
        public bool ProposeOnglerie { get; private set; }
        public bool ProposeMaquillage { get; private set; }
        public List<string> TechniquesEpilation { get; private set; }
        public List<string> TypesSoinsVisage { get; private set; }

        public CabinetEsthetique(
            string nom, string adresse, string ville,
            string telephone, string email,
            bool soinsVisage    = true,
            bool epilation      = true,
            bool onglerie       = false,
            bool maquillage     = false,
            string? description = null)  // ✅ optionnel en dernier
            : base(nom, adresse, ville, telephone, email,
                   CategorieEtablissement.SpaBeaute, // ✅ catégorie fixe
                   description)
        {
            ProposeSoinsVisage  = soinsVisage;
            ProposeEpilation    = epilation;
            ProposeOnglerie     = onglerie;
            ProposeMaquillage   = maquillage;
            TechniquesEpilation = new List<string>();
            TypesSoinsVisage    = new List<string>();
        }

        public void AjouterTechniqueEpilation(string t) => TechniquesEpilation.Add(t);
        public void AjouterSoinVisage(string s) => TypesSoinsVisage.Add(s);
    }
}