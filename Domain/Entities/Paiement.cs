using System;
using Domain.Enum;


namespace Domain.Entities
{
    public class Paiement
    {
        public Guid Id { get; private set; }
        public Guid RendezVousId { get; private set; }
        public Guid ClientId { get; private set; }
        public decimal Montant { get; private set; }
        public string Devise { get; private set; }
        public StatutPaiement Statut { get; private set; }
        public string MethodePaiement { get; private set; }
        // "OrangeMoney", "MtnMomo", "Carte"

        public string? TransactionId { get; private set; }
        // ID retourné par CinetPay après paiement réussi

        public string? LienPaiement { get; private set; }
        // Lien CinetPay vers lequel rediriger le client

        public DateTime DateCreation { get; private set; }
        public DateTime? DateConfirmation { get; private set; }
        
        public Paiement(
            Guid rendezVousId,
            Guid clientId,
            decimal montant,
            string methodePaiement)
        {
            Id               = Guid.NewGuid();
            RendezVousId     = rendezVousId;
            ClientId         = clientId;
            Montant          = montant;
            Devise           = "XAF";
            MethodePaiement  = methodePaiement;
            Statut           = StatutPaiement.EnAttente;
            DateCreation     = DateTime.UtcNow;
        }

        public void DefinirLienPaiement(string lien)
        {
            LienPaiement = lien;
        }

        public void Confirmer(string transactionId)
        {
            TransactionId    = transactionId;
            Statut           = StatutPaiement.Confirme;
            DateConfirmation = DateTime.UtcNow;
        }

        public void Echouer()
        {
            Statut = StatutPaiement.Echoue;
        }

        public void Rembourser()
        {
            Statut = StatutPaiement.Rembourse;
        }
    }

    
}