using LS.API.Models;
using LS.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LS.API.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        // Constructor injecting MediatR mediator for sending commands.
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Endpoint to create a new user.
        [HttpPost("api/users/create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest createUser)
        {
            // Sends a CreateUserCommand through MediatR to handle user creation logic.
            var user = await _mediator.Send(new CreateUserCommand(createUser.Name));
            return Ok(new { message = "User created successfully", user });
        }
    }
}
