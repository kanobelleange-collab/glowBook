using MediatR;
using Application.Features.Rendevou.DTOs;

namespace Application.Features.Rendevou.Queries.GetByClient
{
    public record GetRendezVousByClientQuery(Guid ClientId) : IRequest<List<RendezVousDto>>;
}