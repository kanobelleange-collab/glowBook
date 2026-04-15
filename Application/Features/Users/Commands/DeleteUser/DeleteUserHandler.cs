using Application.Features.Users.Interfaces;
using MediatR;

namespace Application.Features.Users.Commands.DeleteUser
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken ct)
        {
            // 1. Vérifier si l'utilisateur existe
            var user = await _userRepository.GetByIdAsync(request.UserId);

            // Si l'utilisateur n'existe pas, on renvoie false (échec de l'opération)
            if (user == null) return false;

            // 2. Lancer la suppression
            await _userRepository.DeleteUserAsync(user.Id);

            // 3. Retourner true manuellement pour confirmer le succès
            // Puisque DeleteUserAsync renvoie Task (void), on valide ici.
            return true;
        }
    }
}