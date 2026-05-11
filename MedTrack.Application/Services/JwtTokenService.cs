using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace MedTrack.Application.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(string userId, string userName, string role = "User");
        ClaimsPrincipal? ValidateToken(string token);
    }

    public class JwtTokenService : IJwtTokenService
    {
        private readonly string _jwtSecret;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly int _jwtExpirationMinutes;

        public JwtTokenService(IConfiguration configuration)
        {
            _jwtSecret = configuration["Jwt:Secret"] ?? throw new InvalidOperationException("Jwt:Secret not configured");
            _jwtIssuer = configuration["Jwt:Issuer"] ?? "MedTrack";
            _jwtAudience = configuration["Jwt:Audience"] ?? "MedTrackUsers";
            _jwtExpirationMinutes = int.Parse(configuration["Jwt:ExpirationMinutes"] ?? "60");
        }

        public string GenerateToken(string userId, string userName, string role = "User")
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, role),
                new Claim("iss", _jwtIssuer),
                new Claim("aud", _jwtAudience)
            };

            var token = new JwtSecurityToken(
                issuer: _jwtIssuer,
                audience: _jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtExpirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
                var handler = new JwtSecurityTokenHandler();

                var principal = handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = true,
                    ValidIssuer = _jwtIssuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtAudience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
