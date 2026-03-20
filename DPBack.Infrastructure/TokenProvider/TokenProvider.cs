using System.Security.Claims;
using System.Text;
using DPBack.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;
using DPBack.Application.Abstractions;
using DPBack.Application.Options;
using Microsoft.Extensions.Options;

namespace DPBack.Infrastructure.TokenProvider;

public class TokenProvider : ITokenProvider
{
    private readonly JwtOptions _options;

    public TokenProvider( IOptions<JwtOptions> options)
    {
       _options = options.Value;
    }

    public string Create(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));

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

            Expires = DateTime.UtcNow.AddHours(_options.LifetimeInHours),
            SigningCredentials = credentials,
            Issuer = _options.Issuer,
            Audience = _options.Audience,
        };
        var handler = new JsonWebTokenHandler();
        string token = handler.CreateToken(tokenDescriptor);
        return token;
    }
}