using Application.Features.Etablissements.DTOs;
using MediatR;
namespace Application.Features.Etablissements.Queries.GetProximite
{
    public class GetProximiteQuery : IRequest<List<EtablissementDto>>
    {
        public string Ville { get; set; } = string.Empty;
        public string? Quartier { get; set; }
        public double RayonKm { get; set; } = 3;
        public string? TypeServiceNom { get; set; }  // ← remplacé Categorie
    }
}