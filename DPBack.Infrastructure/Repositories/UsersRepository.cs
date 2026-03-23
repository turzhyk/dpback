
using DPBack.Application.Abstractions;
using DPBack.Domain.Models;
using DPBack.Infrastructure.Contexts;
using DPBack.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace DPBack.Infrastructure.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly UserStoreDbContext _context;

    public UsersRepository(UserStoreDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmail(string email, CancellationToken cToken)
    {
        var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cToken);
        if (userEntity == null)
            return null;
        return new User(userEntity.Id, userEntity.Login, userEntity.PasswordHash, userEntity.Email, userEntity.Role,
            userEntity.CreatedAt);
    }

    public async Task<Guid> CreateAsync(User user, CancellationToken cToken)
    {
        var userEntity = new UserEntity(user.Id, user.Login, user.PasswordHash, user.Email, user.Role, user.CreatedAt);
        await _context.Users.AddAsync(userEntity, cToken);
        await _context.SaveChangesAsync(cToken);
        return user.Id;
    }

    public async Task<List<UserAdress>> GetAdressesByUserId(Guid id, CancellationToken cToken)
    {
        var entities = await _context.Adresses.Where(adress => adress.UserId == id).ToListAsync(cToken);
        if (entities.Count == 0)
            return new List<UserAdress>();

        return entities.Select(entity => new UserAdress(entity.Id, entity.UserId, entity.Country, entity.City, entity.Street,
            entity.BuildingNumber, entity.ApartmentNumber, entity.PostalCode, entity.PhoneNumber, entity.Email,
            entity.Options)).ToList();
    }

    public async Task AddUserAdress( UserAdress adress, CancellationToken cToken)
    {
        var entity =  new UserAdressEntity(adress.Id, adress.UserId, adress.Country, adress.City, adress.Street,
            adress.BuildingNumber, adress.ApartmentNumber, adress.PostalCode, adress.PhoneNumber, adress.Email,
            adress.Options);

        await _context.Adresses.AddAsync(entity, cToken);
        await _context.SaveChangesAsync(cToken);
    }
}