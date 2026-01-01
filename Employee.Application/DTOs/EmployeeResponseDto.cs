namespace Employee.Application.DTOs;

public class EmployeeResponseDto
{
    public Guid Id { get; init; }
    public string Code { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string Email { get; init; } = null!;
    public bool IsActive { get; init; }
}
