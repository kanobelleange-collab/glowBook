using Application.Features.Users.Interfaces;
using MediatR;

namespace Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken ct)
        {
            // 1. Récupérer le compte (pour avoir le ReferenceId et le Role)
            var user = await _userRepository.GetByIdAsync(request.UserId);

            // Si le perso n'existe pas en base, on ne peut pas le modder
            if (user == null) return false;

            // 2. Délégation au Repository
            // Le Repo va checker le user.Role et mettre à jour soit la table Clients, soit Employees
            return await _userRepository.UpdateProfileAsync(user, request);
        }
    }
}