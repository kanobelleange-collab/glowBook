using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Application.Features.Payements.DTOs;

namespace Application.Features.Payements.Queries.GetHistoriqueClient
{
    public record GetPaiementsByClientQuery(Guid ClientId) : IRequest<List<PaiementDto>>;
}