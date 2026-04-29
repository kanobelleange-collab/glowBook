using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Features.Etablissements.Commands.CreateEtablissement;
using Application.Features.Etablissements.Commands.UpdateEtablissement;
using Application.Features.Etablissements.Commands.DeleteEtablissement;
using Application.Features.Etablissements.Queries.GetById;
using Application.Features.Etablissements.Queries.GetAll;
using Application.Features.Etablissements.Queries.RechercherEtablissement;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EtablissementsController : ControllerBase
{
    private readonly IMediator _mediator;
    public EtablissementsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll() => Ok(await _mediator.Send(new GetAllEtablissementQuery()));

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id) => Ok(await _mediator.Send(new GetByIdEtablissementQuery { Id = id }));

    [HttpGet("rechercher")]
    [AllowAnonymous]
    public async Task<IActionResult> Rechercher([FromQuery] string? motCle, [FromQuery] string? ville, [FromQuery] string? typeServiceNom)
    {
        return Ok(await _mediator.Send(new RechercherQuery { MotCle = motCle, Ville = ville, TypeServiceNom = typeServiceNom }));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> Create([FromBody] CreateEtablissementCommand command) => Ok(await _mediator.Send(command));

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEtablissementCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);
        return Ok("Mis à jour.");
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteEtablissementCommand { Id = id });
        return Ok("Supprimé.");
    }
}