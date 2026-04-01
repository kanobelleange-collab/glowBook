using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    public class EtablissementService
    {
        public Guid Id { get; private set; }
        public Guid EtablissementId { get; private set; }
        public string TypeServiceNom { get; private set; }
        public List<Prestation> Prestations { get; private set; }
        public List<Employee> Employees { get; private set; } // ✅ Liste des employés du service

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