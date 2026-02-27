using System.Security.Claims;
using System.Text;
using DPBack.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;

namespace DPBack.API.TockenProvider;


public class TokenProvider
{
    public string Create(User user)
    {
        string secretKey = "5ab418f6-5d62-4ae7-8afe-a38c73c72a1e";
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Name, user.Login)
            ]),
            // Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = credentials,
            Issuer = "",
            Audience = "",
        };
        var handler = new JsonWebTokenHandler();
        string token = handler.CreateToken(tokenDescriptor);
        return token;
    }
}