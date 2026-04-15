using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Application.Features.Aviss.Commands.CreerAvis;
using Application.Features.Aviss.Commands.RepondreAvis;
using Application.Features.Aviss.Queries.GetAvisEtablissement;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AvisController : ControllerBase
{
    private readonly IMediator _mediator;
    public AvisController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> LaisserUnAvis([FromBody] CreerAvisCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpGet("etablissement/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByEtablissement(Guid id)
    {
        return Ok(await _mediator.Send(new GetAvisEtablissementQuery(id)));
    }

    [HttpPut("repondre")]
    [Authorize(Roles = "Employee,Admin")]
    public async Task<IActionResult> Repondre([FromBody] RepondreAvisCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
}