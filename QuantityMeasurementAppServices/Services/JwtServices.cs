using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuantityMeasurementAppModels.Entities;

namespace QuantityMeasurementAppServices.Services
{
    public class JwtService
    {
        private readonly IConfiguration configuration;

        public JwtService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GenerateToken(UserEntity user)
        {
            string secretKey   = configuration["Jwt:SecretKey"]!;
            string issuer      = configuration["Jwt:Issuer"]!;
            string audience    = configuration["Jwt:Audience"]!;
            int expiryMinutes  = int.Parse(configuration["Jwt:ExpiryMinutes"]!);

            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            SigningCredentials credentials  = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,   user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString()),
                new Claim("name",   user.Name),
                new Claim("userId", user.Id.ToString()),
                new Claim("role",   user.Role)    // NEW: include role
            };

            JwtSecurityToken jwtToken = new JwtSecurityToken(
                issuer:             issuer,
                audience:           audience,
                claims:             claims,
                expires:            DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        public int GetExpirySeconds()
        {
            int minutes = int.Parse(configuration["Jwt:ExpiryMinutes"]!);
            return minutes * 60;
        }
    }
}