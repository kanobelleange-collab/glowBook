using MediatR;
using Application.Features.Payements.Interfaces;
using Domain.Entities;
using Application.Features.Rendevou.Interfaces;
using  Domain.Enum;
using Application.Features.Payements.DTOs;
using Application.Features.Paiements.Commands.InitialiserPaiement;

public class InitialiserPaiementHandler : IRequestHandler<InitialiserPaiementCommand, PaiementResponseDto>
{
    private readonly IPaiementRepository _repository; // Pour la base de données
    private readonly IPaymentService _paymentService; // Pour l'API CinetPay

    public InitialiserPaiementHandler(IPaiementRepository repository, IPaymentService paymentService)
    {
        _repository = repository;
        _paymentService = paymentService;
    }

    public async Task<PaiementResponseDto> Handle(InitialiserPaiementCommand request, CancellationToken cancellationToken)
    {
        // 1. Créer l'entité (statut "EnAttente")
        var paiement = new Paiement(
            request.RendezVousId, 
            request.ClientId, 
            request.Montant, 
            request.MethodePaiement);

        // 2. APPEL AU SERVICE EXTERNE : IPaymentService
        // On demande à CinetPay de nous générer une session de paiement
        var lienPaiement = await _paymentService.CreerSessionPaiementAsync(
            paiement.RendezVousId,
            paiement.Montant,
            paiement.MethodePaiement,
            "https://glowbook.cm/succes", // Tes URLs de retour
            "https://glowbook.cm/echec"
        );

        // 3. On stocke le lien dans l'entité
        paiement.DefinirLienPaiement(lienPaiement);

        // 4. SAUVEGARDE EN BASE : IPaiementRepository
        await _repository.AddAsync(paiement);

        return new PaiementResponseDto { 
            PaiementId = paiement.Id, 
            LienPaiement = lienPaiement 
        };
    }
}