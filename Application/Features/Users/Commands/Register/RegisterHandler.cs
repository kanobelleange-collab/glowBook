using Application.Features.Users.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Features.Users.Commands.Register
{
    /// <summary>
    /// Handler pour l'inscription, orchestrant la création du compte comme un moddeur assemble un personnage custom.
    /// Le "Double Spawn" : d'abord le profil métier (spawn du personnage), puis le compte sécurité (spawn du compte Rockstar).
    /// </summary>
    public class RegisterHandler : IRequestHandler<RegisterCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public RegisterHandler(IUserRepository userRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }

        public async Task<string> Handle(RegisterCommand request, CancellationToken ct)
        {
            // 1. Vérifier si l'email existe déjà (comme vérifier si un mod est déjà installé)
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Un compte avec cet email existe déjà. Choisissez un autre email, comme un mod unique.");
            }

            // 2. Hasher le mot de passe (sécurité niveau pro, comme crypter un savegame)
            var hashedPassword = _authService.HashPassword(request.Password);

            // 3. Préparer le UserAccount
            var userAccount = new UserAccount
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                PasswordHash = hashedPassword,
                Role = request.Role,
                ReferenceId = Guid.Empty, // Sera défini selon le rôle
                ReferenceType = request.Role
            };

            object? profile = null;

            // 4. Logique du "Double Spawn" selon le rôle
            if (request.Role == "Admin")
            {
                // Admin : Pas de profil métier, juste le compte (comme un admin omnipotent dans un manga)
                userAccount.ReferenceId = Guid.Empty;
                userAccount.ReferenceType = "Admin";
            }
            else if (request.Role == "Client")
            {
                // Client : Créer le profil Client d'abord
                var client = new Client
                {
                    Id = Guid.NewGuid(),
                    Nom = request.Nom,
                    Telephone = request.Telephone,
                    Ville = request.Ville,
                    Quartier = request.Quartier ?? string.Empty,
                    Preferences = request.Preferences ?? new List<string>(),
                    DateInscription = DateTime.UtcNow,
                    EstActif = true
                };
                profile = client;
                userAccount.ReferenceId = client.Id;
                userAccount.ReferenceType = "Client";
            }
            else if (request.Role == "Employee")
            {
                // Employee : Créer le profil Employee d'abord
                if (!request.EtablissementId.HasValue)
                {
                    throw new InvalidOperationException("EtablissementId est requis pour un Employee, comme un clan dans un manga.");
                }
                var employee = new Employee(
                    request.EtablissementId.Value,
                    request.Nom,
                    request.Prenom ?? string.Empty,
                    request.Specialite ?? string.Empty,
                    request.DateHeure ?? string.Empty,
                    request.AnneeExperience ?? 0
                )
                {
                    UrlPhoto = request.UrlPhoto
                };
                profile = employee;
                userAccount.ReferenceId = employee.Id;
                userAccount.ReferenceType = "Employee";
            }
            else
            {
                throw new InvalidOperationException("Rôle invalide. Choisissez 'Client', 'Employee' ou 'Admin', comme choisir une classe dans un RPG.");
            }

            // 5. Inscription transactionnelle (rollback si échec, comme annuler un mod buggé)
            await _userRepository.RegisterAsync(userAccount, profile);

            return $"Inscription réussie ! Bienvenue {request.Nom}, votre compte {request.Role} est prêt, comme un nouveau chapitre dans un manga.";
        }
    }
}