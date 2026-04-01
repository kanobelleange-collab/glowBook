namespace Application.Features.Payements.DTOs;
public class PaiementResponseDto
{
    public Guid PaiementId { get; set; }
    public string LienPaiement { get; set; } = null!;
}