using System;

namespace Domain.Entities
{
    public class Client
    {
        public Guid Id { get; set; }
        public string? Nom { get; set; }
        public string? Telephone { get; set; }
        public string Quartier { get; set; } = string.Empty; // ✅ entré par l'établissement

        public string? Ville { get; set; }
        public List<string> Preferences { get; set; }
        public DateTime DateInscription { get; set; }
        public bool EstActif { get; set; }

        public void AjouterPreference(string p)
        {
            if (!Preferences.Contains(p)) Preferences.Add(p);
        }


    }
}