using MediatR;
namespace Application.Features.Payements.Commands.ConfirmerPaiement
{
    public record ConfirmerPaiementCommand(string TransactionId) : IRequest<bool>;
}