using DPBack.Domain.Enums;

namespace DPBack.Domain.Models
{
   

    public class User
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; private set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<UserAdress> Adresses { get; set; }

        public User()
        {
            
        }
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