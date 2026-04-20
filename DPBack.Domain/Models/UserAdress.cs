namespace DPBack.Domain.Models;

public class UserAdress
{
    public UserAdress(Guid id, Guid userId, string country, string city, string street, string buildingNumber, string apartmentNumber, string postalCode, string phoneNumber, string email, string options)
    {
        Id = id;
        UserId = userId;
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
    public UserAdress(){}
  
    public Guid Id { get; set; }
    public Guid UserId { get; private set; }
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