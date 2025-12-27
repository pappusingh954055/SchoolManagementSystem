using Student.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Student.Domain.Entities
{

    public class Student
    {
        public Guid Id { get; private set; }
        public string StudentCode { get; private set; } = default!;
        public string FirstName { get; private set; } = default!;
        public string LastName { get; private set; } = default!;
        public DateTime DateOfBirth { get; private set; }
        public string Gender { get; private set; } = default!;
        public Guid SchoolId { get; private set; }
        public Address Address { get; private set; } = default!;
        public string? PhotoUrl { get; private set; }

        private Student() { } // EF

        // ✅ CREATE
        public Student(
            string studentCode,
            string firstName,
            string lastName,
            DateTime dob,
            string gender,
            Guid schoolId,
            Address address,
            string? photoUrl)
        {
            Id = Guid.NewGuid();
            StudentCode = studentCode;
            DateOfBirth = dob;
            Gender = gender;
            SchoolId = schoolId;

            UpdateName(firstName, lastName);
            UpdateAddress(address);
            PhotoUrl = photoUrl;
        }

        // ✅ UPDATE METHODS
        public void UpdateName(string firstName, string lastName)
        {
            FirstName = firstName.Trim();
            LastName = lastName.Trim();
        }

        public void UpdateAddress(Address address)
        {
            Address = address;
        }

        public void UpdatePhoto(string? photoUrl)
        {
            PhotoUrl = photoUrl;
        }
    }

}
