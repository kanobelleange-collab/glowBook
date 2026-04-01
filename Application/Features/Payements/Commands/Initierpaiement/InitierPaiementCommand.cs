using System;
using Domain.Entities;
using MediatR;
using Application.Features.Payements.DTOs;


namespace Application.Features.Paiements.Commands.InitialiserPaiement
{
    public class InitialiserPaiementCommand : IRequest<PaiementResponseDto>
    {
        public Guid RendezVousId { get; set; }
        public Guid ClientId { get; set; }
        public decimal Montant { get; set; }
        public string MethodePaiement { get; set; } = "OM"; 
        public string UrlRetourSucces { get; set; } = null!;
        public string UrlRetourEchec { get; set; } = null!;
    }
}