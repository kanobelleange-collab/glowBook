using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Domain.Entities;
using Application.Features.Rendevou.DTOs;

namespace Application.Features.Rendevou.Queries.GetByStatus
{
    public record GetRendezVousByStatutQuery(StatutRendezVous Statut) : IRequest<List<RendezVousDto>>;
}