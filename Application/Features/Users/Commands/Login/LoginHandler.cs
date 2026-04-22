using Application.Features.Users.Dto;
using Application.Features.Users.Interfaces;
using Domain.Entities;
using Domain.Interface;
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
            // 1. Recherche dans UserAccounts
            var userAccount = await _userRepository.GetByEmailAsync(request.Email);
            if (userAccount == null || !_authService.VerifyPassword(request.Password, userAccount.PasswordHash))
            {
                throw new UnauthorizedAccessException("Email ou mot de passe incorrect.");
            }

            // 2. Charger dynamiquement le nom/prénom depuis la table correspondante
            string? nom = null;
            string? prenom = null;
            if (userAccount.ReferenceType == "Client")
            {
                nom = await _userRepository.GetClientNameAsync(userAccount.ReferenceId);
            }
            else if (userAccount.ReferenceType == "Employee")
            {
                var result = await _userRepository.GetEmployeeNameAsync(userAccount.ReferenceId);
                if (result.HasValue)
                {
                    nom = result.Value.Nom;
                    prenom = result.Value.Prenom;
                }
            }

            // 3. Générer le token enrichi
            var token = _authService.GenerateJwtToken(userAccount);

            return new AuthResponseDto(token, userAccount.Email, userAccount.Role, userAccount.Id, nom, prenom);
        }
    }
}