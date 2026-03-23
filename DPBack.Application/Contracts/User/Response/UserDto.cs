using DPBack.Domain.Enums;

namespace DPBack.Application.Contracts;

public class UserDto
{
    public Guid Id { get; set; }
    public string? Login { get; set; }
    public string? Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public UserRole Role { get; set; }
    public List<UserAddressResponseDto>? Addresses { get; set; }

}