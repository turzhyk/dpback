

using DPBack.Domain.Models;

namespace DPBack.Application.Abstractions;

public interface IUsersRepository
{
    Task<Guid> CreateAsync(User user);
    Task<User> GetByEmail(string email);
    Task<List<UserAdress>> GetAdressesByUserId(Guid id);
    Task AddUserAdress( UserAdress adress);
}