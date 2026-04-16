using MediatR;

namespace Application.Features.Admin.Commands.Approvesalon
{
    public record ApproveSalonCommand(Guid SalonId) : IRequest<bool>;
}