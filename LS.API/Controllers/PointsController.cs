using LS.API.Models;
using LS.Application.Commands;
using LS.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LS.API.Controllers
{
    [ApiController]
    public class PointsController : ControllerBase
    {
        private readonly IMediator _mediator;

        // Constructor injecting MediatR mediator for sending commands and queries.
        public PointsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Endpoint for earning points (requires authorization).
        [Authorize]
        [Route("api/users/{userId}/earn")]
        [HttpPost]
        public async Task<IActionResult> EarnPoints(int userId, [FromBody] EarnPointsRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Sends a command to earn points for a specific user.
            await _mediator.Send(new EarnPointsCommand(userId, request.Points));
            return Ok(new { message = "Points earned successfully", userId, points = request.Points });
        }

        // Endpoint for retrieving the total points of a user (anonymous access allowed).
        [AllowAnonymous]
        [HttpGet("api/points/{userId}")]
        public async Task<IActionResult> GetUserPoints(int userId)
        {
            // Sends a query to get the total points for a user.
            var points = await _mediator.Send(new GetUserTotalPointsQuery(userId));
            return Ok(points);
        }
    }
}
