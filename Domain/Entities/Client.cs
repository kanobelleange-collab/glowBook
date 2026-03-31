using System;

namespace Domain.Entities
{
    public class Client
    {
        public Guid Id { get; private set; }
        public string? Nom { get; private set; }
        public string? Email { get; private set; }
        public string? Telephone { get; private set; }
         public string Quartier  { get; set; } = string.Empty; // ✅ entré par l'établissement
       
        public string? Ville { get; private set; }
        public List<string> Preferences { get; private set; }
        public DateTime DateInscription { get; private set; }
        public bool EstActif { get; private set; }

        public void AjouterPreference(string p)
        {
            if (!Preferences.Contains(p)) Preferences.Add(p);
        }

       
    }
}