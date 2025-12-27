using Microsoft.AspNetCore.Http;

namespace Student.API.Models;

public class UpdateStudentRequest
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;

    public string Line1 { get; set; } = default!;
    public string City { get; set; } = default!;
    public string State { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string PostalCode { get; set; } = default!;

    public IFormFile? Photo { get; set; }
}
