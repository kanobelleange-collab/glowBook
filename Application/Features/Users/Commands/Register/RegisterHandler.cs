using Application.Features.Users.Interfaces;
using Domain.Entities;
using MediatR;
using AutoMapper;
using Domain.Enum;

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
        // 0. Garde Fou : Est-ce que le rôle existe ?
        if (!Enum.IsDefined(typeof(UserRole), request.Role))
            throw new ArgumentException("Rôle invalide.");

        // 1. Vérification d'existence
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null) throw new InvalidOperationException("Email déjà utilisé.");

        // 2. Conversion de l'int en Enum (Le Cast)
        var userRole = (Domain.Enum.UserRole)request.Role;

        // 3. Mapping & Sécurité
        var userAccount = _mapper.Map<UserAccount>(request);
        userAccount.Role = userRole; // On fixe le rôle casté
        userAccount.Nom = request.Nom;
        userAccount.PasswordHash = _authService.HashPassword(request.Password);

        // 4. Spawn du profil métier (Switch sur l'Enum casté)
        object? profile = userRole switch
        {
            UserRole.Client => new Client { Id = userAccount.ReferenceId, Nom = request.Nom },
            UserRole.Employee => new Employee { Id = userAccount.ReferenceId, Nom = request.Nom },
            _ => null
        };

        // 5. Persistence
        await _userRepository.RegisterAsync(userAccount, profile);

        return $"Compte {userRole} créé avec succès.";
    }
}