

using DPBack.Domain.Models;

namespace DPBack.Application.Abstractions;

public interface IUsersRepository
{
    Task<Guid> CreateAsync(User user, CancellationToken cToken);
    Task<User?> GetByEmail(string email, CancellationToken cToken);
    Task<bool> UserWithIdExists(Guid id, CancellationToken cToken);
    Task<User?> GetById(Guid id, CancellationToken cToken);
    Task<List<UserAdress>?> GetAdressesByUserId(Guid id, CancellationToken cToken);
    Task AddUserAdress( UserAdress adress, CancellationToken cToken);
}