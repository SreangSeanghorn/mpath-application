

using Microsoft.AspNetCore.Identity;
using MPath.Domain.Core.Primitive;
using MPath.Domain.EventDatas;
using MPath.Domain.Events;
using MPath.Domain.ValueObjects;

namespace MPath.Domain.Entities
{
    public class User : AggregateRoot<Guid>
    {
        public string UserName { get;  set; }
        public Email Email { get;  set; }
        public string Password { get;  set; }
         public ICollection<Role> Roles { get;  set; } = new List<Role>();
         public ICollection<RefreshToken> RefreshTokens { get;  set; } = new List<RefreshToken>();

        private User()
        {
        }
        private User(string userName, Email email, string password)
        {
            UserName = userName;
            Email = email;
            Password = password;
        }
        private User(Guid id, string username, Email email, string password)
        {
            Id = id;
            UserName = username;
            Email = email;
            Password = password;
        }
        public static User Create(string userName, Email email, string password)
        {
            password = new PasswordHasher<User>().HashPassword(null, password);
            return new User(userName, email, password);
        }
        public static User Register(string userName, Email email, string password,Role role)
        {
            var user = Create(userName, email, password);
            user.AssignRole(role);
            var userRegisteredEventData = new UserRegisteredEventData(userName, email.Value, role.Id);
  
            var userRegisteredEvent = new UserRegisteredEvent(user.Id, userRegisteredEventData);
            user.RaiseDomainEvents(userRegisteredEvent);
            return user;
        }
        public bool VerifyPassword(string password)
        {
            var passwordHasher = new PasswordHasher<User>();
            return passwordHasher.VerifyHashedPassword(this, Password, password) == PasswordVerificationResult.Success;
        }


        public void AssignRole(Role role)
        {
            if (Roles.Any(r => r.Name == role.Name)) return;
            Roles.Add(role);
            RaiseRoleAssignedEvent(role.Name);
        }
        public void RaiseRoleAssignedEvent(string role)
        {
            var roleAssignedData = new AssignRoleEventData(Id, role);
            var roleAssignedEvent = new AssignedRoleEvent(Id, roleAssignedData);
            RaiseDomainEvents(roleAssignedEvent);
        }

        public List<string> GetRoles()
        {
            return Roles.Select(r => r.Name).ToList();
        }

        public void AddRefreshToken(RefreshToken refreshToken, DateTime expiryDate)
        {
            RefreshTokens.Add(new RefreshToken(refreshToken.Token, expiryDate));
        }

        public void RevokeRefreshToken(string token)
        {
            var refreshToken = RefreshTokens.SingleOrDefault(rt => rt.Token == token);
            if (refreshToken != null) refreshToken.Revoke();
        }
        public RefreshToken GetValidRefreshToken(string token)
        {
            return RefreshTokens.SingleOrDefault(rt => rt.Token == token && rt.IsValid());
        }

    }
    
}