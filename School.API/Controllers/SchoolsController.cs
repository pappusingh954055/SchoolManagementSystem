using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.Commands.CreateSchool;
using School.Application.Queries.GetSchoolById;

namespace School.API.Controllers;

[ApiController]
[Route("api/schools")]
[Authorize] // 🔐 JWT required
public class SchoolsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SchoolsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // ---------------- Create School (Admin only) ----------------
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateSchoolCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    // ---------------- Get School By Id ----------------
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetSchoolByIdQuery(id));
        if (!result.IsSuccess)
            return NotFound(result.Error);

        return Ok(result.Value);
    }
}
