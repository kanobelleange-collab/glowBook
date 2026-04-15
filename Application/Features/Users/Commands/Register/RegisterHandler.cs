using Application.Features.Users.Commands.Register;
using Application.Features.Users.Interfaces;
using Domain.Entities;
using MediatR;
using AutoMapper;

namespace Application.Features.Users.Commands.Register;

public class RegisterHandler : IRequestHandler<RegisterCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public RegisterHandler(IUserRepository userRepository, IAuthService authService, IMapper mapper)
    {
        _userRepository = userRepository;
        _authService = authService;
        _mapper = mapper;
    }

    public async Task<string> Handle(RegisterCommand request, CancellationToken ct)
    {
        // 1. Check existence (Security check)
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
            throw new InvalidOperationException("Email déjà utilisé.");

        // 2. Mapping Commande -> Entité (Génération Id/ReferenceId via Profile)
        var userAccount = _mapper.Map<UserAccount>(request);

        // 3. Sécurité : Hashage manuel (le mapping ignore PasswordHash)
        userAccount.PasswordHash = _authService.HashPassword(request.Password);

        // 4. Spawn du profil métier minimal (Essential Nom Only)
        object? profile = request.Role switch
        {
            Domain.Enum.UserRole.Client => new Client
            {
                Id = userAccount.ReferenceId,
                Nom = request.Nom
            },
            Domain.Enum.UserRole.Employee => new Employee
            {
                Id = userAccount.ReferenceId,
                Nom = request.Nom
            },
            _ => null // Admin : pas de profil métier lié
        };

        // 5. Persistence Transactionnelle
        await _userRepository.RegisterAsync(userAccount, profile);

        return $"Compte {userAccount.Role} créé avec succès pour {request.Nom}.";
    }
}