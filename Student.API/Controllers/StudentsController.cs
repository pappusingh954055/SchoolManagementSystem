using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student.API.Helpers;
using Student.API.Models;
using Student.Application.Commands.CreateStudent;
using Student.Application.Commands.DeleteStudent;
using Student.Application.Commands.UpdateStudent;
using Student.Application.DTOs;

namespace Student.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StudentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IWebHostEnvironment _env;
    private readonly IFileStorageService _fileStorage;
    public StudentsController(IMediator mediator, IWebHostEnvironment env, IFileStorageService fileStorage)
    {
        _mediator = mediator;
        _env = env;
        _fileStorage = fileStorage;
    }

    // ---------------- CREATE STUDENT ----------------
    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Create([FromForm] CreateStudentRequest request)
    {
        string? photoUrl = null;

        if (request.Photo != null)
        {
            photoUrl = await _fileStorage.SaveStudentPhotoAsync(request.Photo);
        }

        var dto = new CreateStudentDto
        {
            StudentCode = request.StudentCode,
            FirstName = request.FirstName,
            LastName = request.LastName,
            DateOfBirth = request.DateOfBirth,
            Gender = request.Gender,
            SchoolId = request.SchoolId,
            Line1 = request.Line1,
            City = request.City,
            State = request.State,
            Country = request.Country,
            PostalCode = request.PostalCode,
            PhotoUrl = photoUrl
        };

        var result = await _mediator.Send(new CreateStudentCommand(dto));

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    // ---------------- UPDATE STUDENT ----------------
    [HttpPut]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Update([FromForm] UpdateStudentRequest request)
    {
        string? photoUrl = null;

        if (request.Photo != null)
        {
            photoUrl = await _fileStorage.SaveStudentPhotoAsync(request.Photo);
        }

        var dto = new UpdateStudentDto
        {
            Id = request.Id,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Line1 = request.Line1,
            City = request.City,
            State = request.State,
            Country = request.Country,
            PostalCode = request.PostalCode,
            PhotoUrl = photoUrl
        };

        var result = await _mediator.Send(new UpdateStudentCommand(dto));

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    // ---------------- DELETE STUDENT ----------------
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteStudentCommand(id));

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return NoContent();
    }

}
