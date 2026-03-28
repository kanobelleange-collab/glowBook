using System;
using Application.Features.Praticiens.DTOs;

namespace Application.Features.Praticiens.DTOs
{
    public class PraticienDto
    {
        public Guid Id { get; set; }
        public string? Nom { get; set; }
        public  required string Prenom { get; set; }
        public  required string NomComplet { get; set; }
        public  required string Specialite { get; set; }
        public string? Photo { get; set; }
        public string? Description { get; set; }
        public int AnneesExperience { get; set; }
        public double NoteMoyenne { get; set; }
        public Guid EtablissementId { get; set; }
        public List<DisponibiliteDto>? Disponibilites { get; set; }
    }
}