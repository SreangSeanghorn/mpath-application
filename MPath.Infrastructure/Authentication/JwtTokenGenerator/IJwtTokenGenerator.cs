namespace MPath.Infrastructure.Authentication.JwtTokenGenerator
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(string username,List<string> roles);
        string GenerateRefreshToken();
    }
}