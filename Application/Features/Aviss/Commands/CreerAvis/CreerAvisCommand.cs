using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.Aviss.Commands.CreerAvis
{
   public record CreerAvisCommand(
    Guid ClientId, 
    Guid EtablissementId, 
    Guid RendezVousId, 
    int Note, 
    string Commentaire
) : IRequest<Guid>;

}