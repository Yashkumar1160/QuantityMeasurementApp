using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuantityMeasurementAppModels.Entities;

namespace QuantityMeasurementAppServices.Services
{
    // Class that Generates and reads JWT tokens
    public class JwtService
    {
        private readonly IConfiguration configuration;

        // Constructor
        public JwtService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // Method to Generate a signed JWT token for the given user
        public string GenerateToken(UserEntity user)
        {
            // Read config values
            string secretKey     = configuration["Jwt:SecretKey"]!;
            string issuer        = configuration["Jwt:Issuer"]!;
            string audience      = configuration["Jwt:Audience"]!;
            int expiryMinutes    = int.Parse(configuration["Jwt:ExpiryMinutes"]!);

            // Build signing key
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            SigningCredentials credentials  = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            // Add claims
            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,   user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString()),
                new Claim("name",   user.Name),
                new Claim("userId", user.Id.ToString())
            };

            // Build token
            JwtSecurityToken jwtToken = new JwtSecurityToken(
                issuer:             issuer,
                audience:           audience,
                claims:             claims,
                expires:            DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        // Method to Return token lifetime in seconds
        public int GetExpirySeconds()
        {
            int minutes = int.Parse(configuration["Jwt:ExpiryMinutes"]!);
            return minutes * 60;
        }
    }
}