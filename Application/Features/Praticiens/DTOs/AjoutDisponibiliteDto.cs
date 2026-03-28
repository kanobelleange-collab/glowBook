using System;

namespace Application.Features.Praticiens.DTOs
{
    public class DisponibiliteDto
    {
        public Guid PraticienId { get; set; }
        public  required string Jour { get; set; } // "Lundi", "Mardi", etc.
        public TimeSpan HeureDebut { get; set; }
        public TimeSpan HeureFin { get; set; }
    }
}