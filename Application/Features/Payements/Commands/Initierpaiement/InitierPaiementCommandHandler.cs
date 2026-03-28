using MediatR;
using Application.Features.Payements.Interfaces;
using Domain.Entities;
using Application.Features.Rendevou.Interfaces;
using  Domain.Enum;

namespace Application.Features.Payements.Commands
{
    // IRequestHandler<Command, TypeRetour>
    public class InitierPaiementCommandHandler
        : IRequestHandler<InitierPaiementCommand, string>
    {
        private readonly IPaymentService _paymentService;
        private readonly IPaiementRepository _paiementRepository;
        private readonly IRendezVousRepository _rdvRepository;

        public InitierPaiementCommandHandler(
            IPaymentService paymentService,
            IPaiementRepository paiementRepository,
            IRendezVousRepository rdvRepository)
        {
            _paymentService     = paymentService;
            _paiementRepository = paiementRepository;
            _rdvRepository      = rdvRepository;
        }

        // MediatR appelle automatiquement Handle()
        public async Task<string> Handle(
            InitierPaiementCommand command,
            CancellationToken cancellationToken)
        {
            // 1. Vérifier que le RDV existe
            var rdv = await _rdvRepository.GetByIdAsync(command.RendezVousId);
            if (rdv is null)
                throw new Exception("Rendez-vous introuvable.");

            // 2. Vérifier qu'il n'est pas déjà payé
            var paiementExistant = await _paiementRepository
                .GetByRendezVousIdAsync(command.RendezVousId);

            if (paiementExistant != null
                && paiementExistant.Statut == StatutPaiement.Confirme)
                throw new Exception("Ce rendez-vous est déjà payé.");

            // 3. Créer le paiement en base (statut EnAttente)
            var paiement = new Paiement(
                command.RendezVousId,
                command.ClientId,
                command.Montant,
                command.MethodePaiement
            );
            await _paiementRepository.AjouterAsync(paiement);

            // 4. Appeler CinetPay pour obtenir le lien
            string lienPaiement = await _paymentService.CreerSessionPaiementAsync(
                command.RendezVousId,
                command.Montant,
                command.MethodePaiement,
                command.UrlRetourSucces,
                command.UrlRetourEchec
            );

            // 5. Sauvegarder le lien
            paiement.DefinirLienPaiement(lienPaiement);
            await _paiementRepository.MettreAJourAsync(paiement);

            return lienPaiement;
        }
    }
}