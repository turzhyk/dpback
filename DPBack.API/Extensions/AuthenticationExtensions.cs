using System.Text;
using DPBack.Application.Abstractions;
using DPBack.Application.Options;
using DPBack.Infrastructure.TokenProvider;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace DPBack.API.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddAuthorizationServices(this IServiceCollection services, IConfiguration configuration)

    {
        var jwtOptions = configuration.GetSection("Jwt").Get<JwtOptions>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateActor = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret)),
                ValidAudience = jwtOptions.Audience,
                ValidIssuer = jwtOptions.Issuer,
            };
        });
        services.AddAuthorization();
       services.AddSingleton<ITokenProvider,TokenProvider>();
        return services;
    }
}