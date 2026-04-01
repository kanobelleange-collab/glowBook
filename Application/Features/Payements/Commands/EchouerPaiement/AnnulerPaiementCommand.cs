using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.Payements.Commands.EchouerPaiement
{
   public record AnnulerPaiementCommand(Guid PaiementId) : IRequest<bool>;
}