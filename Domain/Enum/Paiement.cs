using System;
using Domain.Entities;

namespace Domain.Enum
{





    public enum StatutPaiement
    {
        EnAttente,   // Paiement initié, client pas encore payé
        Confirme,    // Paiement reçu et validé
        Echoue,      // Paiement échoué
        Rembourse    // Client remboursé suite à annulation
    }
}