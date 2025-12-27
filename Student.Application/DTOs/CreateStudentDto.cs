using System;
using System.Collections.Generic;
using System.Text;

namespace Student.Application.DTOs
{
    public class CreateStudentDto
    {
        public string StudentCode { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = default!;
        public Guid SchoolId { get; set; }

        public string Line1 { get; set; } = default!;
        public string City { get; set; } = default!;
        public string State { get; set; } = default!;
        public string Country { get; set; } = default!;
        public string PostalCode { get; set; } = default!;

        public string? PhotoUrl { get; set; }
    }

}
