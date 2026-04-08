using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    public class EtablissementService
    {
        public Guid Id { get; set; }
        public Guid EtablissementId { get; set; }
        public string TypeServiceNom { get; set; }
        public List<string> SpecialitesTresse { get; set; } = new();
        public List<string> TypesCheveux { get; set; } = new();
        public bool AccepteHommes{get;set;}=true;
        public bool AccepteEnfants{get;set;}=true;
        public string Data { get; set; } = "{}";
        public List<Prestation> Prestations { get; set; }
        public List<Employee> Employees { get; set; } // ✅ Liste des employés du service

        public EtablissementService(Guid etablissementId, string typeServiceNom)
        {
            Id = Guid.NewGuid();
            EtablissementId = etablissementId;
            TypeServiceNom = typeServiceNom;
            Prestations = new List<Prestation>();
            Employees = new List<Employee>(); // initialisation
        }

        public EtablissementService()
        {
            TypeServiceNom = string.Empty;
            Prestations = new List<Prestation>();
            Employees = new List<Employee>();
        }

        public void AjouterPrestation(Prestation prestation)
        {
            if (!Prestations.Any(p => p.Nom == prestation.Nom))
                Prestations.Add(prestation);
        }

        public void AjouterEmployee(Employee employee)
        {
            if (!Employees.Any(e => e.Id == employee.Id))
                Employees.Add(employee);
        }
    }
}