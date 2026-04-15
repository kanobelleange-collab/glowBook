using MediatR;
using Domain.Entities;

namespace Application.Features.Users.Queries.GetAllUsers;

public record GetAllUsersQuery() : IRequest<List<UserAccount>>;