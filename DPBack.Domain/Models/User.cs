using DPBack.Domain.Enums;

namespace DPBack.Domain.Models
{
   

    public class User
    {
        public Guid Id { get; }
        public string Login { get; }
        public string PasswordHash { get; private set; }
        public string Email { get; }
        public UserRole Role { get; }
        public DateTime CreatedAt { get; }
        public List<UserAdress> Adresses { get; }
        
        public User(Guid id, string login, string passwordHash, string email, UserRole role, DateTime createdAt)
        {
            Id = id;
            Login = login;
            PasswordHash = passwordHash;
            Email = email;
            Role = role;
            CreatedAt = createdAt;
        }

        public void SetPassword(string password)
        {
            PasswordHash = password;
        }
    }
}