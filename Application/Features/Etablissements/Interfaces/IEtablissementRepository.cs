using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Application.Features.Etablissements.DTOs;

namespace Application.Features.Etablissements.Interfaces
{
    public interface IEtablissementRepository
    {
        // Lecture
        Task<Etablissement?> GetByIdAsync(Guid id);
        Task<List<Etablissement>> GetAllAsync();
        Task<List<Etablissement>> GetByVilleAsync(string ville);
        Task<List<EtablissementDto>> GetByServiceAsync(string typeServiceNom);
        Task<List<EtablissementDto>> GetMieuxNotesAsync(int top = 10);

        // Recherche
        Task<List<EtablissementDto>> RechercherAsync(
            string? motCle = null,
            string? ville = null,
            string? typeServiceNom = null);

        // Proximité géographique
        Task<List<EtablissementDto>> GetProximiteAsync(
            double latitude,
            double longitude,
            double rayonKm = 3,
            string? typeServiceNom = null);

        // Écriture
        Task AddAsync(Etablissement etablissement);
        Task UpdateAsync(Etablissement etablissement);
        Task DeleteAsync(Guid id);
         // Services
    Task AddServiceAsync(EtablissementService service);
    // Task AddPrestationAsync(Prestation prestation);
    Task<List<EtablissementService>> GetServicesByEtablissementIdAsync(Guid etablissementId);
    Task DeleteServiceAsync(Guid serviceId);

   
    Task<List<Employee>> GetEmployeesByEtablissementIdAsync(Guid etablissementId);
    }
}