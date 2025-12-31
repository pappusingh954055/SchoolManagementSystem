using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.API.Models;
using School.Application.Commands.CreateSchool;
using School.Application.Commands.UpdateSchool;
using School.Application.DTOs;
using School.Application.Interfaces;
using School.Application.Queries.GetSchoolById;

namespace School.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IWebHostEnvironment _env;
        private readonly ISchoolRepository _repository;
        public SchoolsController(IMediator mediator, IWebHostEnvironment env, ISchoolRepository repository)
        {
            _mediator = mediator;
            _env = env;
            _repository = repository;
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


        // ✅ CREATE SCHOOL WITH PHOTO
    
        [HttpPost]
        [Consumes("multipart/form-data")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] CreateSchoolRequest request)
        {
            string? photoUrl = null;

            if (request.Photo != null && request.Photo.Length > 0)
            {
                var extension = Path.GetExtension(request.Photo.FileName)
                    .ToLowerInvariant();

                var allowed = new[] { ".jpg", ".jpeg", ".png" };

                if (!allowed.Contains(extension))
                    return BadRequest("Only JPG / PNG allowed");

                // ✅ WebRootPath SAFETY CHECK (very important in Docker)
                if (string.IsNullOrWhiteSpace(_env.WebRootPath))
                    throw new InvalidOperationException("WebRootPath is not configured");

                var uploadsPath = Path.Combine(
                    _env.WebRootPath,
                    "uploads",
                    "schools");

                // ✅ Ensure directory exists
                Directory.CreateDirectory(uploadsPath);

                var fileName = $"{Guid.NewGuid()}{extension}";
                var fullPath = Path.Combine(uploadsPath, fileName);

                // ✅ Use async + FileAccess.Write (prevents file lock issues)
                await using (var stream = new FileStream(
                    fullPath,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None,
                    bufferSize: 81920,
                    useAsync: true))
                {
                    await request.Photo.CopyToAsync(stream);
                }

                // ✅ STORE ONLY RELATIVE PATH IN DB
                photoUrl = $"/uploads/schools/{fileName}";
            }


            var command = new CreateSchoolCommand(
                new CreateSchoolDto(
                    request.Code,
                    request.Name,
                    request.Line1,
                    request.City,
                    request.State,
                    request.Country,
                    request.PostalCode,
                    photoUrl
                ));

            var result = await _mediator.Send(command);

            return Ok(result.Value);
        }



        // ✅ UPDATE SCHOOL WITH PHOTO
        [HttpPut]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromForm] UpdateSchoolRequest request)
        {
            string? photoUrl = null;

            // ---------------- PHOTO UPLOAD (CONTROLLER RESPONSIBILITY) ----------------
            if (request.Photo != null)
            {
                var extension = Path.GetExtension(request.Photo.FileName).ToLowerInvariant();
                var allowed = new[] { ".jpg", ".jpeg", ".png" };

                if (!allowed.Contains(extension))
                    return BadRequest("Only JPG / PNG allowed");

                var uploadPath = Path.Combine(
                    _env.WebRootPath,
                    "uploads",
                    "schools");

                Directory.CreateDirectory(uploadPath);

                var fileName = $"{Guid.NewGuid()}{extension}";
                var fullPath = Path.Combine(uploadPath, fileName);

                await using var stream = new FileStream(fullPath, FileMode.Create);
                await request.Photo.CopyToAsync(stream);

                photoUrl = $"/uploads/schools/{fileName}";
            }

            // ---------------- SEND COMMAND (CQRS) ----------------
            var command = new UpdateSchoolCommand(
                request.Id,
                request.Name,
                request.Line1,
                request.City,
                request.State,
                request.Country,
                request.PostalCode,
                photoUrl
            );

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteSchoolCommand(id));

            if (!result.IsSuccess)
                return NotFound(result.Error);

            return NoContent();
        }


    }
}
