namespace DPBack.Application.Contracts
{
    public class UserAdressCreateDto
    {
        public UserAdressCreateDto(string country, string city, string street, string buildingNumber,
            string apartmentNumber, string postalCode, string phoneNumber, string email, string options)
        {
            Country = country;
            City = city;
            Street = street;
            BuildingNumber = buildingNumber;
            ApartmentNumber = apartmentNumber;
            PostalCode = postalCode;
            PhoneNumber = phoneNumber;
            Email = email;
            Options = options;
        }
        public string Country { get; private set; }
        public string City { get; private set; }
        public string Street { get; private set; }
        public string BuildingNumber { get; private set; }
        public string ApartmentNumber { get; private set; }
        public string PostalCode { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Email { get; private set; }
        public string Options { get; private set; }
    }
}