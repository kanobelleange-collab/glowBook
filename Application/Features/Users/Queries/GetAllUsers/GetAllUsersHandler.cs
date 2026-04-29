using Application.Features.Users.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, List<UserAccount>>
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersHandler(IUserRepository userRepository) => _userRepository = userRepository;

    public async Task<List<UserAccount>> Handle(GetAllUsersQuery request, CancellationToken ct)
    {
        var users = await _userRepository.GetAllUsersAsync();
        return users.ToList();
    }
}