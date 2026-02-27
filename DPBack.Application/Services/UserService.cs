
using DPBack.Application.Contracts;
using DPBack.Application.Interfaces;
using DPBack.Domain.Models;

namespace DPBack.Application.Services;

public class UserService : IUserService
{
    public readonly IUsersRepository _repo;

    public UserService(IUsersRepository repo)
    {
        _repo = repo;
    }

    public async Task<Guid> CreateUser(User user) => await _repo.CreateAsync(user);
    
    public async Task<User> GetByEmail(string email) => await _repo.GetByEmail(email);

    public async Task<List<UserAddressGetDto>> GetAdressesByUserId(Guid id)
    {
        var entities = await _repo.GetAdressesByUserId(id);

        // Маппинг сущностей в DTO
        return entities.Select(a => new UserAddressGetDto
        (
            a.Country,
             a.City,
            a.Street,
            a.BuildingNumber,
            a.ApartmentNumber,
            a.PostalCode,
            a.PhoneNumber,
            a.Email,
            a.Options
        )).ToList();
    }

    public async Task AddUserAdress(string userId, UserAdressCreateDto dto)
    {
        var userAdress = new UserAdress(Guid.NewGuid(), new Guid(userId), dto.Country, dto.City, dto.Street,
            dto.BuildingNumber, dto.ApartmentNumber, dto.PostalCode, dto.PhoneNumber, dto.Email,
            dto.Options);
        await _repo.AddUserAdress(userAdress);
    }

}