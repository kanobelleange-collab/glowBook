using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Application.Features.Prestations.DTOs;
using Application.Features.Prestations.Commands.CreatePrestation;
using Application.Features.Prestations.Queries.GetById;
using Application.Features.Prestations.Queries.GetServices;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PrestationController : ControllerBase
{
    private readonly IMediator _mediator;
    public PrestationController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<PrestationDto>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetPrestationByIdQuery(id));
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Employee,Admin")]
    public async Task<ActionResult<PrestationDto>> Create([FromBody] CreatePrestationCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
}