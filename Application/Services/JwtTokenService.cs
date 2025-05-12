using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.DTOs;
using Application.Interfaces;
using Application.Settings;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class JwtTokenService(IOptions<JwtSettings> opts) : IJwtTokenService
{
    private readonly JwtSettings _settings = opts.Value;

    public AuthResultDto GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username!),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!)
        };

        var key    = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey!));
        var creds  = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var now    = DateTime.UtcNow;
        var exp    = now.AddMinutes(_settings.AccessTokenExpirationMinutes);

        var jwt = new JwtSecurityToken(
            issuer:             _settings.Issuer,
            audience:           _settings.Audience,
            claims:             claims,
            notBefore:          now,
            expires:            exp,
            signingCredentials: creds
        );

        return new AuthResultDto(
            new JwtSecurityTokenHandler().WriteToken(jwt),
            exp
        );
    }
}