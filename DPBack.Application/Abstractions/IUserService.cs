using DPBack.Application.Contracts;
using DPBack.Domain.Models;


namespace DPBack.Application.Abstractions;

public interface IUserService
{
    Task<Guid> CreateUser(UserCreateRequest request);
    Task<string> Login(UserLoginRequest request);
    Task<User> GetByEmail(string email);
    Task<List<UserAddressGetDto>> GetAdressesByUserId(Guid id);
    Task AddUserAdress(Guid userId, UserAdressCreateDto dto);
}