namespace MPath.Infrastructure.Authentication
{
    public class JwtSettings
    {
        public string SectionName { get; set; } = "JwtSettings";
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiryInMinutes { get; set; }
        public int RefreshTokenExpiryInMinutes { get; set; }
    }
}