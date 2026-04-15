using Application.Features.Users.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

namespace Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _config;

    public AuthService(IConfiguration config)
    {
        _config = config;
    }

    public string HashPassword(string password)
        => BCrypt.Net.BCrypt.HashPassword(password);

    public bool VerifyPassword(string password, string hashedPassword)
        => BCrypt.Net.BCrypt.Verify(password, hashedPassword);

    public string GenerateJwtToken(UserAccount userAccount)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? "Clé_Secrète_De_Secours_32_Caractères_Minimum"));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // ✅ Définition des Claims (Le badge d'identité du joueur)
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userAccount.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("id", userAccount.Id.ToString()),
            new Claim(ClaimTypes.Role, userAccount.Role.ToString()), // ✅ Fix: Utilise l'Enum correctement
            new Claim("refId", userAccount.ReferenceId.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(8), // Session de 8h
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}