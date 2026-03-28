using System;
using MediatR;

namespace Application.Features.Clients.Commands.CreateClient
{
    public class CreateClientCommand : IRequest<Guid>
{
    public string? Nom { get; set; }
    public string? Email { get; set; }
    public string Telephone { get; set; } = string.Empty; // ✅ non-nullable
    public string Ville { get; set; } = string.Empty;     // ✅ non-nullable

    }
}