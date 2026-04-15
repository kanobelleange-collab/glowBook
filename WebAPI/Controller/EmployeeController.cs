using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Features.Employees.DTOs;
using Application.Features.Employees.Commands;
using Application.Features.Employees.Commands.CreerEmployee;
using Application.Features.Employees.Commands.DeleteEmployee;
using Application.Features.Employees.Commands.UpdateEmployee;
using Application.Features.Employees.Queries.GetByEtablissement;
using Application.Features.Employees.Queries.GetBySpecialite;
using Application.Features.Employees.Queries.GetDisponibilite;
using Application.Features.Employees.Queries.GetById;
using Application.Features.Employees.Queries;
using Application.Features.Employees.Commands.AjoutDisponibilite;
using Microsoft.AspNetCore.Authorization; // ✅ Ajouté pour les autorisations

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // ✅ Sécurité globale : il faut être connecté par défaut
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Employee/{id}
        [HttpGet("{id:guid}")]
        [AllowAnonymous] // ✅ Tout le monde peut voir le profil d'un employé
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
        [AllowAnonymous] // ✅ Public pour que les clients voient qui travaille où
        public async Task<ActionResult<List<EmployeeDto>>> GetByEtablissement(Guid etablissementId)
        {
            var employees = await _mediator.Send(new GetEmployeesByEtablissementQuery(etablissementId));
            return Ok(employees);
        }

        // GET: api/Employee/disponibles/{etablissementId}
        [HttpGet("disponibles/{etablissementId:guid}")]
        [AllowAnonymous] // ✅ Indispensable pour la prise de RDV côté client
        public async Task<ActionResult<List<EmployeeDto>>> GetDisponibles(Guid etablissementId, [FromQuery] DateTime dateHeure)
        {
            var employees = await _mediator.Send(new GetAvailableEmployeesQuery(etablissementId, dateHeure));
            return Ok(employees);
        }

        // GET: api/Employee/specialite/{specialite}
        [HttpGet("specialite/{specialite}")]
        [AllowAnonymous] // ✅ Public pour la recherche par spécialité
        public async Task<ActionResult<List<EmployeeDto>>> GetBySpecialite(string specialite)
        {
            var employees = await _mediator.Send(new GetEmployeeBySpecialiteQuery(specialite));
            return Ok(employees);
        }

        // POST: api/Employee
        [HttpPost]
        [Authorize(Roles = "Admin")] // ✅ Seul l'admin peut créer un nouvel employé
        public async Task<ActionResult<EmployeeDto>> Add([FromBody] CreerEmployeeCommand command)
        {
            var employeeDto = await _mediator.Send(command);

            return CreatedAtAction(
                nameof(GetById),
                new { id = employeeDto.Id },
                employeeDto
            );
        }

        [HttpPost("{id:guid}/disponibilites")]
        [Authorize(Roles = "Employee,Admin")] // ✅ L'employé gère son planning ou l'admin
        public async Task<IActionResult> AjouterDisponibilite([FromBody] AjouterDisponibiliteCommand command,
            CancellationToken ct)
        {
            var result = await _mediator.Send(command, ct);
            return Ok(result);
        }

        // PUT: api/Employee/{id}
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Employee,Admin")] // ✅ L'employé modifie ses infos ou l'admin
        public async Task<IActionResult> Update([FromBody] UpdateEmployeeCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        // DELETE: api/Employee/{id}
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")] // ✅ Seul l'admin peut supprimer un compte employé
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteEmployeeCommand(id));
            return NoContent();
        }
    }
}