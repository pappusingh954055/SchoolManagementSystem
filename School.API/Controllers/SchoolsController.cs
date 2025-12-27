using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using School.API.Helpers;
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

        //[HttpPut]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> Update(UpdateSchoolCommand command)
        //{
        //    var result = await _mediator.Send(command);
        //    return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        //}

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteSchoolCommand(id));
            return result.IsSuccess ? Ok() : NotFound(result.Error);
        }

        [HttpPost("{schoolId:guid}/photo")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadPhoto(Guid schoolId, IFormFile file, [FromServices] IWebHostEnvironment env, [FromServices] ISchoolRepository repository)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file");

            var school = await repository.GetByIdAsync(schoolId);
            if (school == null)
                return NotFound("School not found");

            // ✅ Validate file type
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                return BadRequest("Only JPG and PNG files are allowed");

            // ✅ Create uploads folder
            var uploadPath = Path.Combine(
                env.WebRootPath,
                "uploads",
                "schools");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            // ✅ Unique file name
            var fileName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(uploadPath, fileName);

            // ✅ Save file
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // ✅ Save relative URL in DB
            var photoUrl = $"/uploads/schools/{fileName}";
            school.UpdatePhoto(photoUrl);

            await repository.SaveChangesAsync();

            return Ok(new
            {
                PhotoUrl = photoUrl
            });
        }


        // ✅ CREATE SCHOOL WITH PHOTO
    
        [HttpPost]
        //[Consumes("multipart/form-data")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] CreateSchoolRequest request)
        {
            string? photoUrl = null;

            if (request.Photo != null)
            {
                var extension = Path.GetExtension(request.Photo.FileName).ToLowerInvariant();
                var allowed = new[] { ".jpg", ".jpeg", ".png" };

                if (!allowed.Contains(extension))
                    return BadRequest("Only JPG / PNG allowed");

                var uploadsPath = Path.Combine(
                    _env.WebRootPath,
                    "uploads",
                    "schools");

                Directory.CreateDirectory(uploadsPath);

                var fileName = $"{Guid.NewGuid()}{extension}";
                var fullPath = Path.Combine(uploadsPath, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await request.Photo.CopyToAsync(stream);

                // ✅ THIS IS WHAT GOES TO DB
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromForm] UpdateSchoolRequest request)
        {
            var school = await _repository.GetByIdAsync(request.Id);
            if (school == null)
                return NotFound("School not found");

            // ---------------- UPDATE PHOTO (OPTIONAL) ----------------
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

                using var stream = new FileStream(fullPath, FileMode.Create);
                await request.Photo.CopyToAsync(stream);

                var photoUrl = $"/uploads/schools/{fileName}";
                school.UpdatePhoto(photoUrl);
            }

            // ---------------- UPDATE DOMAIN STATE (DDD CORRECT) ----------------
            school.UpdateName(request.Name);

            var address = School.Domain.ValueObjects.Address.Create(
                request.Line1,
                request.City,
                request.State,
                request.Country,
                request.PostalCode
            );

            school.UpdateAddress(address);

            await _repository.SaveChangesAsync();

            return Ok(new
            {
                school.Id,
                school.Code,
                school.Name,
                school.Address.Line1,
                school.Address.City,
                school.Address.State,
                school.Address.Country,
                school.Address.PostalCode,
                school.PhotoUrl
            });
        }

    }
}
