namespace MPath.Infrastructure.Authentication.JwtRefreshTokenGenerator
{
    public interface IJwtRefreshTokenGenerator
    {
        public string GenerateRefreshToken();
        public DateTime GetExpiryDate();
    }
}