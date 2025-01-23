using Microsoft.Extensions.Options;

namespace MPath.Infrastructure.Authentication.JwtRefreshTokenGenerator
{
    public class JwtRefreshTokenGenerator : IJwtRefreshTokenGenerator
    {
        private readonly JwtSettings _jwtSettings;

        public JwtRefreshTokenGenerator(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }
        public string GenerateRefreshToken()
        {
            var random = new byte[32];

            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }

        }

        public DateTime GetExpiryDate()
        {
            return DateTime.UtcNow.AddMinutes(_jwtSettings.RefreshTokenExpiryInMinutes);
        }
    }
}