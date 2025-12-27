namespace School.API.Models
{
    public class CreateSchoolRequest
    {
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Line1 { get; set; } = default!;
        public string City { get; set; } = default!;
        public string State { get; set; } = default!;
        public string Country { get; set; } = default!;
        public string PostalCode { get; set; } = default!;
        public IFormFile? Photo { get; set; }
    }
}
