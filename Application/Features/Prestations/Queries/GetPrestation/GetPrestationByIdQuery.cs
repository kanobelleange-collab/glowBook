using MediatR;
using Application.Features.Prestations.DTOs;

namespace Application.Features.Prestations.Queries.GetPrestation
{
    public record GetPrestationByIdQuery(Guid Id) : IRequest<PrestationDto?>;
}