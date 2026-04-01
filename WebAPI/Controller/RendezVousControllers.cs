using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Rendevou.Commands.CreerRendeVous;
using Application.Features.Rendevou.Commands.AnnulerRendeVous;
using Application.Features.Rendevou.Queries.GetByClient;
using Application.Features.Rendevou.GetRendezVousByPraticien;
using Application.Features.Rendevou.Queries.GetByPraticienDate;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RendezVousController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RendezVousController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // 1. CRÉER UN RENDEZ-VOUS
        [HttpPost]
        public async Task<IActionResult> Creer([FromBody] CreerRendeVousCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // 2. ANNULER UN RENDEZ-VOUS
        [HttpPut("annuler")]
        public async Task<IActionResult> Annuler([FromBody] AnnulerRendeVousCommand command)
        {
            await _mediator.Send(command);
            return NoContent(); // 204: Succès sans contenu de retour
        }

        // 3. HISTORIQUE D'UN CLIENT
        [HttpGet("client/{clientId:guid}")]
        public async Task<IActionResult> GetByClient(Guid clientId)
        {
            var query = new GetRendezVousByClientQuery(clientId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // 4. PLANNING D'UN PRATICIEN
        [HttpGet("praticien/{praticienId:guid}")]
        public async Task<IActionResult> GetByPraticien(Guid praticienId)
        {
            var query = new GetRendezVousByPraticienQuery(praticienId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // 5. AGENDA DU JOUR (PRATICIEN + DATE)
        [HttpGet("agenda/{praticienId:guid}/{date:datetime}")]
        public async Task<IActionResult> GetAgenda(Guid praticienId, DateTime date)
        {
            var query = new GetRendezVousByPraticienDateQuery(praticienId, date);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}