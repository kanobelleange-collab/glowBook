using MediatR;
using Application.Features.Prestations.DTOs;

namespace Application.Features.Prestations.Queries.GetServices
{
    public record GetByServiceQuery(Guid ServiceId) : IRequest<List<PrestationDto>>;
}