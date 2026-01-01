namespace Employee.Application.DTOs;

public class CreateEmployeeRequestDto
{
    public string Code { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string Email { get; init; } = null!;
}
