using Application.Features.Users.Dto;
using Application.Features.Users.Interfaces;
using Domain.Enum;
using MediatR;

namespace Application.Features.Users.Queries.GetUserProfile;

public class GetUserProfileHandler : IRequestHandler<GetUserProfileQuery, UserProfileDto>
{
    private readonly IUserRepository _userRepository;

    public GetUserProfileHandler(IUserRepository userRepository) => _userRepository = userRepository;

    public async Task<UserProfileDto> Handle(GetUserProfileQuery request, CancellationToken ct)
    {
        // 1. On récupère le compte sécu
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null) throw new KeyNotFoundException("Utilisateur non trouvé.");

        string nom = "Inconnu";

        // 2. Switch sur le rôle pour extraire les infos métier
        // Rappel : On a simplifié au maximum, on ne prend que le NOM ici pour coller à ton DTO
        switch (user.Role)
        {
            case UserRole.Client:
                nom = await _userRepository.GetClientNameAsync(user.ReferenceId) ?? "Client";
                break;

            case UserRole.Employee:
                var emp = await _userRepository.GetEmployeeNameAsync(user.ReferenceId);
                nom = emp?.Nom ?? "Employé";
                break;

            case UserRole.Admin:
                nom = "Administrateur";
                break;
        }

        // 3. Retourne le DTO (UserId, Email, Role, Nom)
        return new UserProfileDto(user.Id, user.Email, user.Role, nom);
    }
}