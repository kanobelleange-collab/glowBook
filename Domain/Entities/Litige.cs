using Domain.Enum;

namespace Domain.Entities
{
    public class Litige
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid RendezVousId { get; set; }
        public Guid EmetteurId { get; set; } // ID du Client ou du Salon qui signale
        public string Motif { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public StatutLitige Statut { get; set; } = StatutLitige.Ouvert;
        public DateTime DateCreation { get; set; } = DateTime.UtcNow;
    }
}