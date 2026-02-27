namespace DPBack.Domain.Models
{
    public enum UserRole
    {
        User,
        Worker,
        Admin,
    }

    public class User
    {
        public Guid Id { get; }
        public string Login { get; }
        public string PasswordHash { get; }
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
    }
}