using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RestaurantOS.Application.Auth;
using RestaurantOS.Domain.Entities;

namespace RestaurantOS.Infrastructure.Auth;

public class TokenService(IOptions<JwtSettings> opt) : ITokenService
{
    private readonly JwtSettings _s = opt.Value;

    public string CreateAccessToken(ApplicationUser user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim("displayName", user.DisplayName),
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_s.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _s.Issuer, audience: _s.Audience, claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_s.AccessTokenMinutes), signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}