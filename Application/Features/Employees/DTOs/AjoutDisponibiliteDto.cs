using System;

namespace Application.Features.Employees.DTOs
{
    public class DisponibiliteDto
    {
        public Guid EmployeeId { get; set; }
        public  required string Jour { get; set; } // "Lundi", "Mardi", etc.
        public TimeSpan HeureDebut { get; set; }
        public TimeSpan HeureFin { get; set; }
    }
}