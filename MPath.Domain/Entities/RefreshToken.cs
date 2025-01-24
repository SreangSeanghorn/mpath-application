


using MPath.SharedKernel.Primitive;

namespace MPath.Domain.Entities
{
    public class RefreshToken : Entity<Guid>
    {
        public Guid Id { get; private set; }
        public string Token { get; private set; }
        public DateTime ExpiryDate { get; private set; }
        public bool IsRevoked { get; private set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiryDate;

        private RefreshToken(string token, DateTime expiryDate)
        {
            Token = token ?? throw new ArgumentNullException(nameof(token));
            ExpiryDate = expiryDate;
            IsRevoked = false;
        }
        
        public static RefreshToken Create(string token, DateTime expiryDate)
        {
            return new RefreshToken(token, expiryDate);
        }

        public void Revoke()
        {
            if (IsRevoked) throw new InvalidOperationException("Token is already revoked.");
            IsRevoked = true;
        }

        public bool IsValid()
        {
            return !IsRevoked && !IsExpired;
        }
    }

}