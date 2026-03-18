using System.Text.Json.Serialization;

namespace DPBack.Application.Contracts;


public class UserLoginRequest
{
    public string Login { get; set; }
    public string Password { get; set; }
}