namespace Application.Features.Payements.DTOs;
public class PaiementDto
{
    public Guid Id { get; set; }
    public Guid RendezVousId { get; set; }
    public decimal Montant { get; set; }
    public string Devise { get; set; } = "XAF";
    public string Statut { get; set; } = null!;
    public string MethodePaiement { get; set; } = null!;
    public string? TransactionId { get; set; }
    public string? LienPaiement { get; set; }
    public DateTime DateCreation { get; set; }
}