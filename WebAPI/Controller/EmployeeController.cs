using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Features.Employees.DTOs;
using Application.Features.Employees.Commands;
using  Application.Features.Employees.Commands.CreerEmployee;
using Application.Features.Employees.Commands.DeleteEmployee;
using Application.Features.Employees.Commands.UpdateEmployee;
using Application.Features.Employees.Queries.GetByEtablissement;
using Application.Features.Employees.Queries.GetBySpecialite;
using Application.Features.Employees.Queries.GetDisponibilite;
using Application.Features.Employees.Queries.GetById;
using Application.Features.Employees.Queries;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Employee/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeDto>> GetById(Guid id)
        {
            var query = new GetEmployeeByIdQuery(id);
            var employee = await _mediator.Send(query);
            
            if (employee == null) 
                return NotFound($"L'employé avec l'ID {id} n'existe pas.");

            return Ok(employee);
        }

        // GET: api/Employee/etablissement/{etablissementId}
        [HttpGet("etablissement/{etablissementId:guid}")]
        public async Task<ActionResult<List<EmployeeDto>>> GetByEtablissement(Guid etablissementId)
        {
            var employees = await _mediator.Send(new GetEmployeesByEtablissementQuery(etablissementId));
            return Ok(employees);
        }

        // GET: api/Employee/disponibles/{etablissementId}
        [HttpGet("disponibles/{etablissementId:guid}")]
        public async Task<ActionResult<List<EmployeeDto>>> GetDisponibles(Guid etablissementId, [FromQuery] DateTime dateHeure)
        {
            // Note: dateHeure sera récupérée depuis la query string (?dateHeure=...)
            var employees = await _mediator.Send(new GetAvailableEmployeesQuery(etablissementId, dateHeure));
            return Ok(employees);
        }

        // GET: api/Employee/specialite/{specialite}
        [HttpGet("specialite/{specialite}")]
        public async Task<ActionResult<List<EmployeeDto>>> GetBySpecialite(string specialite)
        {
            var employees = await _mediator.Send(new GetEmployeeBySpecialiteQuery(specialite));
            return Ok(employees);
        }

        // POST: api/Employee
        [HttpPost]
        public async Task<ActionResult<Guid>> Add([FromBody] CreerEmployeeCommand command)
        {
            var id = await _mediator.Send(command);
            // Retourne un code 201 avec le lien vers le GetById
            return CreatedAtAction(nameof(GetById), new { id = id }, id);
        }

        // PUT: api/Employee/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEmployeeCommand command)
        {
            // Sécurité : on s'assure que l'ID de l'URL est celui utilisé
            if (id != command.Id) 
                return BadRequest("L'ID fourni dans l'URL ne correspond pas à l'ID de l'objet.");

            await _mediator.Send(command);
            return NoContent();
        }

        // DELETE: api/Employee/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteEmployeeCommand(id));
            return NoContent();
        }
    }
}