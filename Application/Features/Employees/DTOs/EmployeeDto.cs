using System;
using Application.Features.Employees.DTOs;

namespace Application.Features.Employees.DTOs
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public Guid EtablissementId { get; set; }
        public string? Nom { get; set; }
        public  required string Prenom { get; set; }
        public  required string Specialite { get; set; }
        public string? Photo { get; set; }
        public string DateCreation{get;set;}=string.Empty;
        public int AnneesExperience { get; set; }
        public double NoteMoyenne { get; set; }
        
        public List<DisponibiliteDto>? Disponibilites { get; set; }
    }
}