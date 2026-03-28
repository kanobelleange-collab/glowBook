using MediatR;
using Application.Features.ServiceEsthetiques.DTOs;

namespace Application.Features.ServiceEsthetiques.Queries.GetAll.GetServiceByCategories
{
    public class GetServicesByCategorieQuery : IRequest<List<ServiceEsthetiqueDto>>
    {
        public required string Categorie { get; set; }
        // "Tressage", "Massage", "Soin visage", "Onglerie", "Épilation"
    }
}
    