using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Features.Clients.Commands.CreateClient;
using Application.Features.Clients.Commands.UpdateClient;
using Application.Features.Clients.Commands.DeleteClient;
using Application.Features.Clients.Queries.GetAll;
using Application.Features.Clients.Queries.GetById;
using Application.Features.Clients.Queries.GetClientByEmail;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // 1. Récupérer tous les clients
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _mediator.Send(new GetAllClientQuery());
            return Ok(response);
        }

        // 2. Récupérer un client par son ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _mediator.Send(new GetByIdClientQuery(id));
            return response != null ? Ok(response) : NotFound();
        }

        // 3. Récupérer un client par son Email
        [HttpGet("by-email/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var response = await _mediator.Send(new GetClientByEmailQuery(email));
            return response != null ? Ok(response) : NotFound();
        }

        // 4. Créer un client
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClientCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = id }, command);
        }

        // 5. Mettre à jour un client
        [HttpPut("{id}")]
        public async Task<IActionResult> Update( [FromBody] UpdateClientCommand command)
        {
            
            await _mediator.Send(command);
            return NoContent();
        }

        // 6. Supprimer un client
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteClientCommand(id));
            return NoContent();
        }
    }
}