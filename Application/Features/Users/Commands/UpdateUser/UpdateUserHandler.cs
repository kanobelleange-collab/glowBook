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
            // 1. Récupérer le compte pour connaître le Role et le ReferenceId
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null) return false;

            // 2. Mise à jour selon le rôle
            // On délègue la mise à jour lourde au repository pour rester Clean
            return await _userRepository.UpdateProfileAsync(user, request);
        }
    }
}