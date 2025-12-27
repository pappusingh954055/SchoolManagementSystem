namespace School.Domain.ValueObjects;

public sealed class SchoolCode
{
    public string Value { get; }

    private SchoolCode() { } // EF Core

    public SchoolCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("School code cannot be empty");

        Value = value.Trim().ToUpperInvariant();
    }

    public override string ToString() => Value;
}
