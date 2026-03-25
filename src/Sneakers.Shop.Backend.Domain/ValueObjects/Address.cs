using Sneakers.Shop.Backend.Domain.Exceptions;

namespace Sneakers.Shop.Backend.Domain.ValueObjects
{
    public record Address
    {
        public string Country { get; }
        public string State { get; }
        public string City { get; }
        public string Street { get; }
        public string HouseNumber { get; }
        public int PostalCode { get; }

        public Address(
        string country,
        string state,
        string city,
        string street,
        string houseNumber,
        int postalCode)
        {
            if (string.IsNullOrWhiteSpace(country))
                throw new DomainException("Country cannot be empty.", nameof(country));
            if (string.IsNullOrWhiteSpace(state))
                throw new DomainException("State cannot be empty.", nameof(state));
            if (string.IsNullOrWhiteSpace(city))
                throw new DomainException("City cannot be empty.", nameof(city));
            if (string.IsNullOrWhiteSpace(street))
                throw new DomainException("Street cannot be empty.", nameof(street));
            if (string.IsNullOrWhiteSpace(houseNumber))
                throw new DomainException("House number cannot be empty.", nameof(houseNumber));

            if (postalCode <= 0 || postalCode.ToString().Length is < 5 or > 6)
                throw new DomainException("Postal code must be a positive number with 5-6 digits.", nameof(postalCode));

            Country = country.Trim();
            State = state.Trim();
            City = city.Trim();
            Street = street.Trim();
            HouseNumber = houseNumber.Trim();
            PostalCode = postalCode;
        }

        public override string ToString() => $"{Country}, {State}, {City}, {Street} {HouseNumber}, {PostalCode}";
    }
}
