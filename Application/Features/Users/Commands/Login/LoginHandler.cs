using Application.Features.Users.Dto;
using Application.Features.Users.Interfaces;
using Domain.Enum;
using MediatR;

namespace Application.Features.Users.Commands.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, AuthResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public LoginHandler(IUserRepository userRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }

        public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken ct)
        {
            // 1. Recherche du compte par email
            var userAccount = await _userRepository.GetByEmailAsync(request.Email);

            if (userAccount is null)
            {
                throw new UnauthorizedAccessException("Email ou mot de passe incorrect.");
            }

            if (!_authService.VerifyPassword(request.Password, userAccount.PasswordHash))
            {
                throw new UnauthorizedAccessException("Email ou mot de passe incorrect.");
            }

            // 2. Récupération du Nom selon le rôle
            var nom = userAccount.Role switch
            {
                UserRole.Client => await _userRepository.GetClientNameAsync(userAccount.ReferenceId),
                UserRole.Employee => (await _userRepository.GetEmployeeNameAsync(userAccount.ReferenceId))?.Nom,
                _ => null
            };

            // 3. Génération du Token JWT
            var token = _authService.GenerateJwtToken(userAccount);

            return new AuthResponseDto(
                token,
                userAccount.Email,
                userAccount.Role.ToString(),
                userAccount.Id,
                nom
            );
        }
    }
}