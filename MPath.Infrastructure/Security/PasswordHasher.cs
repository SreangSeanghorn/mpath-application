

using System.Security.Cryptography;
using System.Text;
using MPath.Domain.Core.Interfaces;

namespace MPath.Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
    }

    public bool VerifyPassword(string hashedPassword, string password)
    {
        var hashedProvidedPassword = HashPassword(password);
        return hashedPassword == hashedProvidedPassword;
    }
}