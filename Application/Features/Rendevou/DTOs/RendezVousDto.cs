using System;
using Domain.Entities;

namespace Application.Features.Rendevou.DTOs
{
    public class RendezVousDto
    {
        public Guid Id { get; set; }

        // Client
        public Guid ClientId { get; set; }
        public string ClientNom { get; set; } = string.Empty;
        public string ClientEmail { get; set; } = string.Empty;

        // Employee
        public Guid EmployeeId { get; set; }
        public string EmployeeNom { get; set; } = string.Empty;

        // Service
        public Guid ServiceId { get; set; }
        public string ServiceNom { get; set; } = string.Empty;

        // Établissement
        public Guid EtablissementId { get; set; }
        public string EtablissementNom { get; set; } = string.Empty;

        // RDV
        public DateTime DateHeure { get; set; }
        public decimal Prix { get; set; }
        public string? NotesClient { get; set; }
        public string? RaisonAnnulation { get; set; }
        public StatutRendezVous Statut { get; set; }
        public DateTime DateCreation { get; set; }
    }
}