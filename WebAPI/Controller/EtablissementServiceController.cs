using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Features.Etablissements.Commands.AddServices.SalonCoiffure;
using Application.Features.Etablissements.Commands.AddServices.SalonMassage;

namespace API.Controllers;

[ApiController]
[Route("api/etablissements/{id}/services")]
[Authorize(Roles = "Admin,Employee")]
public class EtablissementServicesController : ControllerBase
{
    private readonly IMediator _mediator;
    public EtablissementServicesController(IMediator mediator) => _mediator = mediator;

    [HttpPost("salon-coiffure")]
    public async Task<IActionResult> AjouterSalonCoiffure(Guid id, [FromBody] AddSalonCoiffureCommand command)
    {
        command.EtablissementId = id;
        return Ok(new { serviceId = await _mediator.Send(command) });
    }

    [HttpPost("salon-massage")]
    public async Task<IActionResult> AjouterSalonMassage(Guid id, [FromBody] AddSalonMassageCommand command)
    {
        command.EtablissementId = id;
        return Ok(new { serviceId = await _mediator.Send(command) });
    }
}