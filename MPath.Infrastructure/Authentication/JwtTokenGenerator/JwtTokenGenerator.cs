
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MPath.Infrastructure.Authentication.JwtTokenGenerator
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtSettings _jwtSettings;

        public JwtTokenGenerator(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }
        public string GenerateToken(string username, List<string> roles)
        {
            Console.WriteLine("Roles: " + string.Join(",", roles.Count));
            // create a claim based on the username
            var claims = new[]
            {
               new Claim(JwtRegisteredClaimNames.Sub, username),
               new Claim(ClaimTypes.Name, username),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }
            .Union(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            var signingKey = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret))
            , SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
                signingCredentials:  signingKey
            );
               
            Console.WriteLine("Token: " + new JwtSecurityTokenHandler().WriteToken(token));
            return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

}