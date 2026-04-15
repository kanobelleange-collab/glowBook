using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Application.Features.Clients.Commands.CreateClient;
using Application.Features.Clients.Commands.UpdateClient;
using Application.Features.Clients.Commands.DeleteClient;
using Application.Features.Clients.Queries.GetAll;
using Application.Features.Clients.Queries.GetById;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientController : ControllerBase
{
    private readonly IMediator _mediator;
    public ClientController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _mediator.Send(new GetAllClientQuery()));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await _mediator.Send(new GetByIdClientQuery(id));
        return response != null ? Ok(response) : NotFound();
    }



    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromBody] CreateClientCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, command);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateClientCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteClientCommand(id));
        return NoContent();
    }
}