namespace School.Domain.ValueObjects;

public class Address
{
    public string Line1 { get; private set; } = default!;
    public string City { get; private set; } = default!;
    public string State { get; private set; } = default!;
    public string Country { get; private set; } = default!;
    public string PostalCode { get; private set; } = default!;

    private Address() { } // EF

    private Address(string line1, string city, string state, string country, string postalCode)
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

        return new Address(line1, city, state, country, postalCode);
    }
}
