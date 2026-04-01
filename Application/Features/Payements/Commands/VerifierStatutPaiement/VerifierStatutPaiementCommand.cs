using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.Payements.VerifierStatutPaiement
{
    public record VerifierStatutPaiementCommand(string TransactionId) : IRequest<bool>;
}