using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Kukirmash.Core.Models;
using Kukirmash.Infrastructure.JWT;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;


namespace Kukirmash.Infrastructure;

public class JwtProvider
{
    private readonly JwtOptions _options;

    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public string GenerateToken(User user)
    {
        Claim[] claims = [new("userId", user.Id.ToString())];

        var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddHours(_options.ExpiresHours));

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenString;
    }
}
