using System.Security.Claims;
using System.Text;
using DPBack.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace DPBack.API.TockenProvider;


public class TokenProvider
{
    public string Create(User user, IConfiguration configuration)
    {
        string secretKey = configuration["JWT:Secret"];
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Name, user.Login),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            ]),
            Expires = DateTime.UtcNow.AddHours(configuration.GetValue<int>("JWT:TokenLifetimeInHours")),
            SigningCredentials = credentials,
            Issuer = configuration["JWT:Issuer"],
            Audience = configuration["JWT:Audience"],
        };
        var handler = new JsonWebTokenHandler();
        string token = handler.CreateToken(tokenDescriptor);
        return token;
    }
}