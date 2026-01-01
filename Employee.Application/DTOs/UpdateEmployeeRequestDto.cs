namespace Employee.Application.DTOs;

public class UpdateEmployeeRequestDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string Email { get; init; } = null!;
}
