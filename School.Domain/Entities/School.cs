using School.Domain.ValueObjects;

namespace School.Domain.Entities;

public class School
{
    public Guid Id { get; private set; }
    public string Code { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public Address Address { get; private set; } = default!;
    public string? PhotoUrl { get; private set; }

    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

    private School() { } // EF

    public School(string code, string name, Address address, string? photoUrl = null)
    {
        Id = Guid.NewGuid();
        Code = code;
        UpdateName(name);
        Address = address;
        PhotoUrl = photoUrl;
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("School name cannot be empty");

        Name = name.Trim();
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
