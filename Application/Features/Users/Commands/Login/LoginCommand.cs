using MediatR;
using Application.Features.Users.Dto;

namespace Application.Features.Users.Commands.Login
{
    public record LoginCommand(string Email, string Password) : IRequest<AuthResponseDto>;
}