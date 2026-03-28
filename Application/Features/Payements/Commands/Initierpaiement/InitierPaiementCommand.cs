using System;
using Domain.Entities;
using MediatR;


namespace Application.Features.Payements.Commands

{
    public class InitierPaiementCommand:IRequest<string>
    {
        public Guid RendezVousId { get; set; }
        public Guid ClientId { get; set; }
        public decimal Montant { get; set; }
        public  required string MethodePaiement { get; set; }
        // "OrangeMoney", "MtnMomo", "Carte", "All"
        public string? UrlRetourSucces { get; set; }
        public string? UrlRetourEchec { get; set; }
    }
}