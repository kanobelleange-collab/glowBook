using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Etablissements.Commands.CreateEtablissement;
using Application.Features.Etablissements.Commands.UpdateEtablissement;
using Application.Features.Etablissements.Commands.DeleteEtablissement;
using Application.Features.Etablissements.Queries.GetById;
using Application.Features.Etablissements.Queries.GetAll;
using Application.Features.Etablissements.Queries.GetByVille;
using Application.Features.Etablissements.Queries.RechercherEtablissement;
using Application.Features.Etablissements.Queries.GetMieuxNotes;
using Application.Features.Etablissements.Queries.GetProximite;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EtablissementsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EtablissementsController(IMediator mediator)
            => _mediator = mediator;

        // GET api/etablissements
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllEtablissementQuery());
            return Ok(result);
        }

        // GET api/etablissements/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(
                new GetByIdEtablissementQuery { Id = id });
            return Ok(result);
        }

        // GET api/etablissements/ville?ville=Douala
        [HttpGet("ville")]
        public async Task<IActionResult> GetByVille([FromQuery] string ville)
        {
            var result = await _mediator.Send(
                new GetByVilleQuery { Ville = ville });
            return Ok(result);
        }

        // GET api/etablissements/rechercher?motCle=afro&ville=Douala&typeServiceNom=SalonCoiffure
        [HttpGet("rechercher")]
        public async Task<IActionResult> Rechercher(
            [FromQuery] string? motCle         = null,
            [FromQuery] string? ville          = null,
            [FromQuery] string? typeServiceNom = null)
        {
            var result = await _mediator.Send(new RechercherQuery
            {
                MotCle         = motCle,
                Ville          = ville,
                TypeServiceNom = typeServiceNom
            });
            return Ok(result);
        }

        // GET api/etablissements/mieux-notes?top=5
        [HttpGet("mieux-notes")]
        public async Task<IActionResult> GetMieuxNotes([FromQuery] int top = 10)
        {
            var result = await _mediator.Send(
                new GetMieuxNotesQuery { Top = top });
            return Ok(result);
        }

        // GET api/etablissements/proximite?ville=Douala&rayonKm=3&typeServiceNom=SalonMassage
        [HttpGet("proximite")]
        public async Task<IActionResult> GetProximite(
            [FromQuery] string ville,
            [FromQuery] string? quartier       = null,
            [FromQuery] double rayonKm         = 3,
            [FromQuery] string? typeServiceNom = null)
        {
            var result = await _mediator.Send(new GetProximiteQuery
            {
                Ville          = ville,
                Quartier       = quartier,
                RayonKm        = rayonKm,
                TypeServiceNom = typeServiceNom
            });
            return Ok(result);
        }

        // POST api/etablissements
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateEtablissementCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // PUT api/etablissements/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateEtablissementCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);
            return Ok("Établissement mis à jour avec succès.");
        }

        // DELETE api/etablissements/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteEtablissementCommand { Id = id });
            return Ok("Établissement supprimé avec succès.");
        }
    }
}