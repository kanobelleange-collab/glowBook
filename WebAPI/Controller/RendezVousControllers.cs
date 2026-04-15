using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Features.Rendevou.Commands.CreerRendeVous;
using Application.Features.Rendevou.Commands.AnnulerRendeVous;
using Application.Features.Rendevou.Queries.GetByClient;
using Application.Features.Rendevou.GetRendezVousByEmployer;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RendezVousController : ControllerBase
{
    private readonly IMediator _mediator;
    public RendezVousController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> Creer([FromBody] CreerRendeVousCommand command) => Ok(await _mediator.Send(command));

    [HttpGet("client/{clientId:guid}")]
    public async Task<IActionResult> GetByClient(Guid clientId) => Ok(await _mediator.Send(new GetRendezVousByClientQuery(clientId)));

    [HttpGet("praticien/{praticienId:guid}")]
    [Authorize(Roles = "Employee,Admin")]
    public async Task<IActionResult> GetByPraticien(Guid praticienId) => Ok(await _mediator.Send(new GetRendezVousByEmployeeQuery(praticienId)));
}