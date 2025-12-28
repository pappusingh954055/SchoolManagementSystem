using Identity.API.Contracts;
using Identity.Application.Commands.RefreshToken;
using Identity.Application.Commands.RegisterUser;
using Identity.Application.Queries.LoginUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginUserQuery query)
    {
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return Unauthorized(result.Error);

        return Ok(result.Value);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(
    [FromBody] string refreshToken)
    {
        // Will be implemented fully later
        return Ok("Refresh token logic ready");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("secure")]
    public IActionResult Secure()
    {
        return Ok("Admin access granted");
    }

    // ---------------- REFRESH TOKEN ----------------
    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh(
        [FromBody] RefreshTokenRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
            return BadRequest("Refresh token is required");

        var result = await _mediator.Send(
            new RefreshTokenCommand(request.RefreshToken));

        if (!result.IsSuccess)
            return Unauthorized(result.Error);

        return Ok(result.Value);
    }
}
