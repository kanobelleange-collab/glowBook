using System;
using Application.Features.Employees.Interfaces;
using Application.Features.Employees.DTOs;
using Domain.Entities;

namespace Application.Features.Employees.Interfaces
{

    public interface IEmployeeRepository
    {
        // Récupérer un praticien par son Id
        Task<Employee?> GetByIdAsync(Guid id);

        // Récupérer tous les praticiens d'un établissement
        Task<List<EmployeeDto>> GetByEtablissementAsync(Guid etablissementId);

        // Récupérer les praticiens disponibles à une date/heure précise
        Task<List<EmployeeDto>> GetDisponiblesAsync(Guid etablissementId, DateTime dateHeure);

        // Récupérer par spécialité
        Task<List<EmployeeDto>> GetBySpecialiteAsync(string specialite);

        // Ajouter un praticien
        Task AddAsync(Employee employee);
        Task AddDisponibiliteAsync(DisponibiliteDto disponibilite);

        // Mettre à jour un praticien
        Task <EmployeeDto>UpdateAsync(Employee employee);

        // Supprimer un praticien
        Task DeleteAsync(Guid id);
    }
}