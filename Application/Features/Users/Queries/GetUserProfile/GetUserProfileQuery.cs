using Application.Features.Users.Dto;
using MediatR;

namespace Application.Features.Users.Queries.GetUserProfile;

public record GetUserProfileQuery(Guid UserId) : IRequest<UserProfileDto>;