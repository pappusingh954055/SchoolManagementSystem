namespace Employee.Domain.Entities;

public class Employee
{
    public Guid Id { get; private set; }
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public bool IsActive { get; private set; }

    private Employee() { } // EF Core

    public Employee(string code, string name, string email)
    {
        Id = Guid.NewGuid();
        Code = code;
        Name = name;
        Email = email;
        IsActive = true;
    }
}
