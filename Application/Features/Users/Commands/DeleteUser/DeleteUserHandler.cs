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
            if (user == null) return false;

            // 2. Lancer la suppression (Le Repo gérera la transaction)
            return await _userRepository.DeleteUserAsync(user.Id);
        }
    }
}