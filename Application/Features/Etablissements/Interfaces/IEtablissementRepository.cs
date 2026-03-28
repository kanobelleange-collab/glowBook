// Application/Features/Etablissements/Interfaces/IEtablissementRepository.cs
using Domain.Entities;

namespace Application.Features.Etablissements.Interfaces
{
    public interface IEtablissementRepository
    {
        Task<Etablissement?> GetByIdAsync(Guid id);
        Task<List<Etablissement>> GetAllAsync();
        Task<List<Etablissement>> GetByVilleAsync(string ville);

        // ✅ Categorie devient un enum
        Task<List<Etablissement>> GetByCategorieAsync(CategorieEtablissement categorie);

        Task<List<Etablissement>> RechercherAsync(string motCle, string? ville = null);
        Task<List<Etablissement>> GetMieuxNotesAsync(int top = 10);
        Task AddAsync(Etablissement etablissement);
        Task UpdateAsync(Etablissement etablissement);
        Task DeleteAsync(Guid id);
    }
}