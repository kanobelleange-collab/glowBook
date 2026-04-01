using Application.Features.Paiements.Commands.InitialiserPaiement;
using Application.Features.Payements.Commands.ConfirmerPaiement;
using Application.Features.Payements.Queries.GetPaiementByRendezVous;
using Application.Features.Payements.Queries.GetHistoriqueClient;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaiementController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaiementController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // 1. Initialiser un paiement (Appelé par l'App Mobile/Web)
        [HttpPost("initialiser")]
        public async Task<IActionResult> Initialiser([FromBody] InitialiserPaiementCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        // 2. Récupérer le statut d'un paiement pour un RDV
        [HttpGet("rendezvous/{rendezVousId}")]
        public async Task<IActionResult> GetByRendezVous(Guid rendezVousId)
        {
            var result = await _mediator.Send(new GetPaiementByRendezVousQuery(rendezVousId));
            if (result == null) return NotFound();
            return Ok(result);
        }

        // 3. Historique des paiements d'un client
        [HttpGet("client/{clientId}")]
        public async Task<IActionResult> GetByClient(Guid clientId)
        {
            var result = await _mediator.Send(new GetPaiementsByClientQuery(clientId));
            return Ok(result);
        }

        // 4. LE WEBHOOK (Très important pour CinetPay)
        // C'est cette URL que CinetPay appellera automatiquement en arrière-plan
        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook([FromForm] string cpm_trans_id)
        {
            // CinetPay envoie souvent les données en format Form-Data
            if (string.IsNullOrEmpty(cpm_trans_id)) return BadRequest();

            var result = await _mediator.Send(new ConfirmerPaiementCommand(cpm_trans_id));
            
            return result ? Ok() : BadRequest();
        }
    }
}