using System;


namespace Application.Features.ServiceEsthetiques.DTOs
{
    public class ServiceEsthetiqueDto
    {
        public Guid Id { get; set; }
        public  required string Nom { get; set; }
        public required string Description { get; set; }
        public required  decimal Prix { get; set; }
        public  required int DureeEnMinutes { get; set; }
        public required  string Categorie { get; set; }
        public Guid EtablissementId { get; set; }
    }
}