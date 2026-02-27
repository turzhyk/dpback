using System.ComponentModel.DataAnnotations;
namespace DPBack.Application.Contracts;

public class UserCreateRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required ]
    public string Password { get; set; }
}