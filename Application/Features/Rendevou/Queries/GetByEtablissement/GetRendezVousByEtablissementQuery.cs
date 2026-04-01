using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Application.Features.Rendevou.DTOs;

namespace Application.Features.Rendevou.Queries.GetByEtablissement
{
   public record GetRendezVousByEtablissementQuery(Guid EtablissementId) : IRequest<List<RendezVousDto>>;
}