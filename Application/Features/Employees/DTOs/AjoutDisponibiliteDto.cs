using System;

namespace Application.Features.Employees.DTOs
{
    public class DisponibiliteDto
    {
        public Guid EmployeeId { get; set; }
        public  required string Jour { get; set; } // "Lundi", "Mardi", etc.
        public string HeureDebut { get; set; }=string.Empty;
        public string HeureFin { get; set; }=string.Empty;
    }
}