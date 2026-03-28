using System;

namespace Domain.Entities
{
    public class Client
    {
        public Guid Id { get; private set; }
        public string? Nom { get; private set; }
        public string? Email { get; private set; }
        public string Telephone { get; private set; }
       
        public string Ville { get; private set; }
        public List<string> Preferences { get; private set; }
        public DateTime DateInscription { get; private set; }
        public bool EstActif { get; private set; }

        public Client(string? nom, string? email, string telephone, string ville)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("L'email est obligatoire.");

            Id              = Guid.NewGuid();
            Nom             = nom;
            Email           = email;
            Telephone       = telephone;
            Ville           = ville;
            Preferences     = new List<string>();
            DateInscription = DateTime.UtcNow;
            EstActif        = true;
        }

        public void AjouterPreference(string p)
        {
            if (!Preferences.Contains(p)) Preferences.Add(p);
        }

       
        public string NomComplet => $" {Nom}";
    }
}