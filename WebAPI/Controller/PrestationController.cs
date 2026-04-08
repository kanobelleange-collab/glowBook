using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Features.Prestations.DTOs;
using Application.Features.Prestations.Commands.CreatePrestation;
using Application.Features.Prestations.Commands.UpdatePrestation;
using Application.Features.Prestations.Queries.GetByEtablissement;
using Application.Features.Prestations.Queries.GetById;
using Application.Features.Prestations.Queries.GetServices;


namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrestationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PrestationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ✅ GET BY ID
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PrestationDto>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetPrestationByIdQuery(id));

            if (result == null)
                return NotFound($"Prestation {id} introuvable");

            return Ok(result);
        }

        // ✅ GET BY SERVICE
        [HttpGet("service/{serviceId:guid}")]
        public async Task<ActionResult<List<PrestationDto>>> GetByService(Guid serviceId)
        {
            var result = await _mediator.Send(new GetByServiceQuery(serviceId));
            return Ok(result);
        }

        // ✅ CREATE
        [HttpPost]
        public async Task<ActionResult<PrestationDto>> Create([FromBody] CreatePrestationCommand command)
        {
            var result = await _mediator.Send(command);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result
            );
        }

        // ✅ UPDATE
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<PrestationDto>> Update(Guid id, [FromBody] UpdatePrestationCommand command)
        {
            command.Id = id;

            var result = await _mediator.Send(command);

            return Ok(result);
        }

        /*// ✅ DELETE
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeletePrestationCommand(id));
            return NoContent();
        }*/
    }
}