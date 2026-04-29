using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Application.Features.Paiements.Commands.InitialiserPaiement;
using Application.Features.Payements.Commands.ConfirmerPaiement;
using Application.Features.Payements.Queries.GetHistoriqueClient;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaiementController : ControllerBase
{
    private readonly IMediator _mediator;
    public PaiementController(IMediator mediator) => _mediator = mediator;

    [HttpPost("initialiser")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> Initialiser([FromBody] InitialiserPaiementCommand command) => Ok(await _mediator.Send(command));

    [HttpPost("webhook")]
    [AllowAnonymous]
    public async Task<IActionResult> Webhook([FromForm] string cpm_trans_id)
    {
        if (string.IsNullOrEmpty(cpm_trans_id)) return BadRequest();
        var result = await _mediator.Send(new ConfirmerPaiementCommand(cpm_trans_id));
        return result ? Ok() : BadRequest();
    }
}