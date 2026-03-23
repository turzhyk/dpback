using DPBack.Application.Contracts;
using DPBack.Domain.Models;


namespace DPBack.Application.Abstractions;

public interface IUserService
{
    Task<Guid> CreateUser(UserCreateRequest request, CancellationToken cToken);
    Task<string> Login(UserLoginRequest request, CancellationToken cToken);
    Task<UserDto> GetByEmail(string email, CancellationToken cToken);
    Task<List<UserAddressResponseDto>> GetAddressesByUserId(Guid id, CancellationToken cToken);
    Task AddUserAddress(Guid userId, UserAdressCreateDto dto, CancellationToken cToken);
}