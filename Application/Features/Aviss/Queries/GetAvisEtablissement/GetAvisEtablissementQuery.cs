using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Aviss.DTOs;
using MediatR;

namespace Application.Features.Aviss.Queries.GetAvisEtablissement
{
    public record GetAvisEtablissementQuery(Guid EtablissementId) : IRequest<AvisEtablissementResponse>;
    public record AvisEtablissementResponse(double NoteMoyenne, List<AvisDto> Avis);
}