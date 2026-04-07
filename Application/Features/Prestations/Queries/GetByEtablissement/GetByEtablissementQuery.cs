using MediatR;

using Application.Features.Prestations.DTOs;



namespace Application.Features.Prestations.Queries.GetByEtablissement
{
    // GetByEtablissementQuery.cs
public record GetByEtablissementQuery(Guid EtablissementId) : IRequest<List<PrestationDto>>;

}