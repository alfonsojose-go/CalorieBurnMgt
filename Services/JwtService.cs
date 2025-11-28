using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using CalorieBurnMgt.Models;

namespace CalorieBurnMgt.Services
{
    public class JwtService
    {
        private readonly string _secret;
        private readonly double _expirationMinutes;

        public JwtService(string secret, double expirationMinutes = 60)
        {
            if (string.IsNullOrEmpty(secret) || secret.Length < 32)
                throw new ArgumentException("JWT Secret MUST be at least 32 characters!");
            _secret = secret;
            _expirationMinutes = expirationMinutes;
        }

        // Generate JWT
        public string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("name", user.FullName)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_expirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Validate JWT and return Name
        public string? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secret);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
                return nameClaim;
            }
            catch
            {
                return null;
            }
        }
    }
}
