using DPBack.Application.Contracts;
using DPBack.Domain.Models;


namespace DPBack.Application.Interfaces;

public interface IUserService
{
    Task<Guid> CreateUser(User user);
    Task<User> GetByEmail(string email);
    Task<List<UserAddressGetDto>> GetAdressesByUserId(Guid id);
    Task AddUserAdress(string userId, UserAdressCreateDto dto);
}