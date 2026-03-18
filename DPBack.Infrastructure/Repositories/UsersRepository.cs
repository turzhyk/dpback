
using DPBack.Application.Abstractions;
using DPBack.Domain.Models;
using DPBack.Infrastructure.Contexts;
using DPBack.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace DPBack.Infrastructure.Repositories;

public class UsersRepository : IUsersRepository
{
    public readonly UserStoreDbContext _context;

    public UsersRepository(UserStoreDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetByEmail(string email)
    {
        var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (userEntity == null)
            return null;
        return new User(userEntity.Id, userEntity.Login, userEntity.PasswordHash, userEntity.Email, userEntity.Role,
            userEntity.CreatedAt);
    }

    public async Task<Guid> CreateAsync(User user)
    {
        var userEntity = new UserEntity(user.Id, user.Login, user.PasswordHash, user.Email, user.Role, user.CreatedAt);
        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();
        return user.Id;
    }

    public async Task<List<UserAdress>> GetAdressesByUserId(Guid id)
    {
        var entities = await _context.Adresses.Where(adress => adress.UserId == id).ToListAsync();
        if (entities == null)
            throw new Exception($"no address fount for user {id}");

        return entities.Select(entity => new UserAdress(entity.Id, entity.UserId, entity.Country, entity.City, entity.Street,
            entity.BuildingNumber, entity.ApartmentNumber, entity.PostalCode, entity.PhoneNumber, entity.Email,
            entity.Options)).ToList();
    }

    public async Task AddUserAdress( UserAdress adress)
    {
        var entity =  new UserAdressEntity(adress.Id, adress.UserId, adress.Country, adress.City, adress.Street,
            adress.BuildingNumber, adress.ApartmentNumber, adress.PostalCode, adress.PhoneNumber, adress.Email,
            adress.Options);

        await _context.Adresses.AddAsync(entity);
        await _context.SaveChangesAsync();
    }
}