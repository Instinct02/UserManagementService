using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagementService.Api.CQRS.Commands;
using UserManagementService.Api.CQRS.Queries;
using UserManagementService.Api.Models;

namespace UserManagementService.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var query = new UsersQuery { PageNumber = pageNumber, PageSize = pageSize };
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
    {
        if (user == null || user.Id != id)
        {
            return BadRequest("User data is invalid");
        }
        // Assuming you have a command to handle user updates
        var command = new UpdateUserCommand(user);
        await mediator.Send(command);
        return NoContent();
    }
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        // Assuming you have a command to handle user deletion
        var command = new DeleteUserCommand(id);
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResult), 200)]
    public async Task<IActionResult> RegisterUserAsync(RegisterUserCommand command)
    {
        var response = await mediator.Send(command);
        return Ok(response);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResult), 200)]
    public async Task<IActionResult> LoginUserAsync(LoginUserCommand command)
    {
        var response = await mediator.Send(command);
        return Ok(response);
    }
}