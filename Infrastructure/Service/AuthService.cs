using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Features.Users.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BC = BCrypt.Net.BCrypt;

namespace Infrastructure.Service
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;

        public AuthService(IConfiguration config) => _config = config;

        public string HashPassword(string password) => BC.HashPassword(password);

        // ✅ Correction CS0117 : Utilisation de la méthode synchrone Verify
        public bool VerifyPassword(string password, string hashedPassword)
            => BC.Verify(password, hashedPassword);

        public string GenerateJwtToken(UserAccount userAccount)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Récupération de la clé depuis le appsettings.json
            var secretKey = _config["Jwt:Key"] ?? "Clef_Secrete_De_Secours_Trop_Courte_Minimum_32_Chars";
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("userId", userAccount.Id.ToString()),
                    new Claim(ClaimTypes.Email, userAccount.Email),
                    new Claim(ClaimTypes.Role, userAccount.Role),
                    new Claim("refId", userAccount.ReferenceId.ToString()),
                    new Claim("refType", userAccount.ReferenceType)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}