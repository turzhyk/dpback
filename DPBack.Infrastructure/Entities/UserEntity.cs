using DPBack.Domain.Models;

namespace DPBack.Infrastructure.Entities;



public class UserEntity
{
    public Guid Id { get; private set; }
    public string Login { get; private set; }
    public string PasswordHash { get; private set; }
    public string Email { get; private set; }
    public UserRole Role { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public List<UserAdressEntity> Adresses { get; private set; }

    private UserEntity() { } 
    public UserEntity(Guid id, string login, string passwordHash, string email, UserRole role, DateTime createdAt)
    {
        Id = id;
        Login = login;
        PasswordHash = passwordHash;
        Email = email;
        Role = role;
        CreatedAt = createdAt;
    }
}