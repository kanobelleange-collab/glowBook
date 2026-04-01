using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.Aviss.Commands.RepondreAvis
{
    public record RepondreAvisCommand(Guid AvisId, string Reponse) : IRequest<bool>;
}