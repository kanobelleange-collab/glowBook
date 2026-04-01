using System;

namespace Domain.Entities
{
    public class EtablissementService
    {
        public Guid Id { get; private set; }
        public Guid EtablissementId { get; private set; }
        public string TypeServiceNom { get; private set; }
        public List<Prestation> Prestations { get; private set; }

        public EtablissementService(Guid etablissementId, string typeServiceNom)
        {
            Id              = Guid.NewGuid();
            EtablissementId = etablissementId;
            TypeServiceNom  = typeServiceNom;
            Prestations     = new List<Prestation>();
        }

        public EtablissementService()
        {
            TypeServiceNom = string.Empty;
            Prestations    = new List<Prestation>();
        }
         public void AjouterPrestation(Prestation prestation)
        {
            if (!Prestations.Any(p => p.Nom == prestation.Nom))
                Prestations.Add(prestation);
        }
    }
}