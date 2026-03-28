using System;
using Domain.Entities;


namespace Application.Features.Rendevou.DTOs
{
public class RendezVousDto
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public Guid PraticienId { get; set; }
        public Guid ServiceId { get; set; }
        public Guid EtablissementId { get; set; }
        public DateTime DateHeure { get; set; }
        public decimal Prix { get; set; }
        public string? NotesClient { get; set; }
        public StatutRendezVous Statut { get; set; }

    }
}