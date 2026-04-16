// Application/Features/Admin/Queries/GetActiveLitiges/GetActiveLitigesQuery.cs
using Domain.Entities;
using MediatR;

namespace Application.Features.Admin.Queries
{
    /// <summary>
    /// Query pour obtenir les litiges non résolus.
    /// </summary>
    public record GetActiveLitigesQuery : IRequest<List<Litige>>;
}