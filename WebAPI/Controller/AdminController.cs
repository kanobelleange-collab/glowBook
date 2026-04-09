using Application.Features.Admin.Commands;
using Application.Features.Admin.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Contrôleur pour les opérations d'administration, réservé aux Game Masters comme dans un serveur de jeu.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Obtenir les statistiques globales du système.
        /// </summary>
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var stats = await _mediator.Send(new GetAdminStatsQuery());
            return Ok(stats);
        }

        /// <summary>
        /// Obtenir la liste des salons en attente d'approbation.
        /// </summary>
        [HttpGet("pending-salons")]
        public async Task<IActionResult> GetPendingSalons()
        {
            var salons = await _mediator.Send(new GetPendingSalonsQuery());
            return Ok(salons);
        }

        /// <summary>
        /// Approuver un salon.
        /// </summary>
        [HttpPost("approve-salon/{salonId}")]
        public async Task<IActionResult> ApproveSalon(Guid salonId)
        {
            try
            {
                var success = await _mediator.Send(new ApproveSalonCommand(salonId));
                return Ok(new { Message = "Salon approuvé avec succès." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Bannir ou débannir un utilisateur.
        /// </summary>
        [HttpPost("ban-user/{userId}")]
        public async Task<IActionResult> BanUser(Guid userId, [FromBody] bool isActive)
        {
            try
            {
                var success = await _mediator.Send(new BanUserCommand(userId, isActive));
                var action = isActive ? "débanni" : "banni";
                return Ok(new { Message = $"Utilisateur {action} avec succès." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }
    }
}