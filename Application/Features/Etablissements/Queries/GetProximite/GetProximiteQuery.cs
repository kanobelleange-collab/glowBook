// Application/Features/Etablissements/Queries/GetProximite/GetProximiteQuery.cs
using Application.Features.Etablissements.DTOs;
using Domain.Entities;
using MediatR;

namespace Application.Features.Etablissements.Queries.GetProximite
{
    public class GetProximiteQuery : IRequest<List<EtablissementDto>>
    {
        public string Ville     { get; set; } = string.Empty; // ✅ entré par le client
        public string? Quartier { get; set; }                 // ✅ entré par le client
        public double RayonKm   { get; set; } = 3;
        public CategorieEtablissement? Categorie { get; set; }
        // ❌ Pas de Latitude/Longitude — géocodé automatiquement
    }
}