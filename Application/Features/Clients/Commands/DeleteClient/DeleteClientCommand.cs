using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.Clients.Commands.DeleteClient;

  public record DeleteClientCommand(Guid Id) : IRequest<Guid>;


