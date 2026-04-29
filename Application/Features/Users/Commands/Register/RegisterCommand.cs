using MediatR;

namespace Application.Features.Users.Commands.Register;

public record RegisterCommand : IRequest<string>
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public int Role { get; set; }
    public required string Nom { get; set; }
}