namespace School.Domain.ValueObjects;

public sealed class Address
{
    public string Line1 { get; }
    public string City { get; }
    public string State { get; }
    public string Country { get; }
    public string PostalCode { get; }

    private Address() { }

    private Address(
        string line1,
        string city,
        string state,
        string country,
        string postalCode)
    {
        Line1 = line1;
        City = city;
        State = state;
        Country = country;
        PostalCode = postalCode;
    }

    public static Address Create(
        string line1,
        string city,
        string state,
        string country,
        string postalCode)
    {
        if (string.IsNullOrWhiteSpace(line1))
            throw new ArgumentException("Address Line1 required");

        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City required");

        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentException("Country required");

        if (string.IsNullOrWhiteSpace(postalCode))
            throw new ArgumentException("Postal code is required");

        return new Address(
            line1.Trim(),
            city.Trim(),
            state?.Trim(),
            country.Trim(),
            postalCode?.Trim()
        );
    }
}
