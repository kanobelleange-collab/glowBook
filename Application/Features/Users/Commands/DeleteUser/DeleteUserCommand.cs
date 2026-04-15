using MediatR;

namespace Application.Features.Users.Commands.DeleteUser;

// Retourne bool (true si suppression réussie)
public record DeleteUserCommand(Guid UserId) : IRequest<bool>;