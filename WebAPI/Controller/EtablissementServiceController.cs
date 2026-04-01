using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Etablissements.Commands.AddServices.SalonCoiffure;
using Application.Features.Etablissements.Commands.AddServices.CabinetProthetiste;
using Application.Features.Etablissements.Commands.AddServices.SalonMassage;
using Application.Features.Etablissements.Commands.AddServices.SpaBeaute;
using Application.Features.Etablissements.Commands.AddServices.CabinetEsthetique;


namespace API.Controllers
{
    [ApiController]
    [Route("api/etablissements/{id}/services")]
    public class EtablissementServicesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EtablissementServicesController(IMediator mediator)
            => _mediator = mediator;

        [HttpPost("salon-coiffure")]
        public async Task<IActionResult> AjouterSalonCoiffure(
            Guid id, [FromBody] AddSalonCoiffureCommand command)
        {
            command.EtablissementId = id;
            var serviceId = await _mediator.Send(command);
            return Ok(new { serviceId });
        }

        [HttpPost("salon-massage")]
        public async Task<IActionResult> AjouterSalonMassage(
            Guid id, [FromBody] AddSalonMassageCommand command)
        {
            command.EtablissementId = id;
            var serviceId = await _mediator.Send(command);
            return Ok(new { serviceId });
        }

        [HttpPost("cabinet-esthetique")]
        public async Task<IActionResult> AjouterCabinetEsthetique(
            Guid id, [FromBody] AddCabinetEsthetiqueCommand command)
        {
            command.EtablissementId = id;
            var serviceId = await _mediator.Send(command);
            return Ok(new { serviceId });
        }

        [HttpPost("cabinet-prothetiste")]
        public async Task<IActionResult> AjouterCabinetProthetiste(
            Guid id, [FromBody] AddCabinetProthetisteCommand command)
        {
            command.EtablissementId = id;
            var serviceId = await _mediator.Send(command);
            return Ok(new { serviceId });
        }

        [HttpPost("spa-beaute")]
        public async Task<IActionResult> AjouterSpaBeaute(
            Guid id, [FromBody] AddSpaBeauteCommand command)
        {
            command.EtablissementId = id;
            var serviceId = await _mediator.Send(command);
            return Ok(new { serviceId });
        }
    }
}