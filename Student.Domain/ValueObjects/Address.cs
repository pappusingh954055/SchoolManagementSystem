using System;
using System.Collections.Generic;
using System.Text;

namespace Student.Domain.ValueObjects
{    

    public class Address
    {
        public string Line1 { get; private set; } = default!;
        public string City { get; private set; } = default!;
        public string State { get; private set; } = default!;
        public string Country { get; private set; } = default!;
        public string PostalCode { get; private set; } = default!;

        private Address() { }

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
            => new Address(line1, city, state, country, postalCode);
    }

}
