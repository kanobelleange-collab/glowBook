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
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null) throw new KeyNotFoundException("Utilisateur non trouvé.");

        string nom = "Inconnu";
        string prenom = "";

        // Même switch que dans le Login pour charger le profil métier
        switch (user.Role)
        {
            case UserRole.Client:
                var client = await _userRepository.GetClientNameAsync(user.ReferenceId);
                if (client.HasValue) { nom = client.Value.Nom; prenom = client.Value.Prenom; }
                break;

            case UserRole.Employee:
                var emp = await _userRepository.GetEmployeeNameAsync(user.ReferenceId);
                if (emp.HasValue) { nom = emp.Value.Nom; prenom = emp.Value.Prenom; }
                break;

            case UserRole.Admin:
                nom = "Administrateur";
                break;
        }

        return new UserProfileDto(user.Id, user.Email, user.Role.ToString(), nom, prenom);
    }
}