using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Application.Features.Rendevou.DTOs;

namespace Application.Features.Rendevou.Queries.GetByPraticienDate
{
   public record GetRendezVousByPraticienDateQuery(Guid PraticienId, DateTime Date) : IRequest<List<RendezVousDto>>;
}