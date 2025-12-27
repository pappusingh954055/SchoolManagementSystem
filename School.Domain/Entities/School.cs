using School.Domain.ValueObjects;

namespace School.Domain.Entities;

public class School
{
    public Guid Id { get; private set; }

    public SchoolCode Code { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public string Line1 { get; set; } = default!;
    public string City { get; set; } = default!;
    public string State { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string PostalCode { get; set; } = default!;

    private School() { } // EF Core

    public School(SchoolCode code, string name, string line1, string city, string state, string country, string postalcode)
    {
        Id = Guid.NewGuid();
        Code = code;
        Name = name;
        Line1 = line1;
        City = city;
        State = state;
        Country = country;
        PostalCode = postalcode;
    }
}
