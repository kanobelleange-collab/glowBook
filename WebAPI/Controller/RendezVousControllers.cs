using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Features.Rendevou.Commands.CreerRendeVous;
using Application.Features.Rendevou.Commands.AnnulerRendeVous;
using Application.Features.Rendevou.Queries.GetByClient;
using Application.Features.Rendevou.GetRendezVousByEmployer;
using Application.Features.RendeVou.Queries;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RendezVousController : ControllerBase
{
    private readonly IMediator _mediator;
    public RendezVousController(IMediator mediator) => _mediator = mediator;

    // POST /api/RendezVous
    [HttpPost]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> Creer([FromBody] CreerRendeVousCommand command)
        => Ok(await _mediator.Send(command));

    // GET /api/RendezVous/client/{clientId}
    [HttpGet("client/{clientId:guid}")]
    public async Task<IActionResult> GetByClient(Guid clientId)
        => Ok(await _mediator.Send(new GetRendezVousByClientQuery(clientId)));

    // GET /api/RendezVous/employee/{employeeId}
    // ✅ Renommé "praticien" → "employee" pour éviter le conflit de route
    [HttpGet("employee/{employeeId:guid}")]
    [Authorize(Roles = "Employee,Admin")]
    public async Task<IActionResult> GetByEmployee(Guid employeeId)
        => Ok(await _mediator.Send(new GetRendezVousByEmployeeQuery(employeeId)));

    // GET /api/RendezVous/agenda/{etablissementId}/{date}
    [HttpGet("agenda/{etablissementId:guid}/{date}")]
    [Authorize(Roles = "Employee,Admin")]
    public async Task<IActionResult> GetAgenda(Guid etablissementId, DateTime date)
    {
        var agenda = await _mediator.Send(new GetAgendaQuery
        {
            EtablissementId = etablissementId,
            Date = date
        });
        return Ok(agenda);
    }
}