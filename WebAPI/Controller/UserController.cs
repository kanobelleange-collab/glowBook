using Application.Features.Users.Commands.UpdateUser;
using Application.Features.Users.Commands.DeleteUser;
using Application.Features.Users.Queries.GetAllUsers;
using Application.Features.Users.Queries.GetUserProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/users")]
[Authorize] // Sécurité globale pour ce controller
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator) => _mediator = mediator;

    // --- 🔍 LECTURE ---

    [HttpGet]
    [Authorize(Roles = "Admin")] // Seul l'Admin voit tout le monde
    public async Task<IActionResult> GetAll()
    {
        var users = await _mediator.Send(new GetAllUsersQuery());
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProfile(Guid id)
    {
        var profile = await _mediator.Send(new GetUserProfileQuery(id));
        return Ok(profile);
    }

    // --- ✍️ ACTIONS ---

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateUserCommand command)
    {
        var success = await _mediator.Send(command);
        return success ? Ok(new { Message = "Profil mis à jour" }) : NotFound();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _mediator.Send(new DeleteUserCommand(id));
        return success ? Ok(new { Message = "Utilisateur supprimé" }) : NotFound();
    }
}