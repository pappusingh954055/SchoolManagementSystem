namespace Identity.Domain.Entities;

public class UserRole
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = default!;

    public Guid UserId { get; private set; }
    public User User { get; private set; } = default!;

    private UserRole() { } // EF

    public UserRole(string name)
    {
        Name = name;
    }
}
