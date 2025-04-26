using Moq;
using MPath.Domain.Core.Interfaces;
using MPath.Domain.Entities;
using MPath.Domain.Events;
using MPath.Domain.ValueObjects;

namespace MPath.Tests.DomainTests.Users;

public class UserTests
{
    [Fact]
    public void Create_Should_CreateUserSuccessfully()
    {
        // Arrange
        var email = Email.Create("test@example.com");
        var password = "SecurePass123";
        var userName = "TestUser";
        
        // Act
        var user = User.Create(userName, email, password);
        
        // Assert
        Assert.NotNull(user);
        Assert.Equal(userName, user.UserName);
        Assert.Equal(email, user.Email);
        Assert.Equal(password, user.Password);
    }
    
    [Fact]
    public void Register_Should_RegisterUserWithRoleAndRaiseEvent()
    {
        // Arrange
        var email = Email.Create("test@example.com");
        var password = "SecurePass123";
        var userName = "TestUser";
        var role = Role.CreateRole("Admin", "Admin role");
    
        // Act
        var user = User.Register(userName, email, password, role);
    
        // Assert
        Assert.NotNull(user);
        Assert.Contains(role, user.Roles);
    
        // Ensure two events are raised
        var events = user.GetDomainEvents();
        Assert.Equal(2, events.Count);
    
        Assert.Contains(events, e => e is UserRegisteredEvent);
        Assert.Contains(events, e => e is AssignedRoleEvent);
    }
    
    [Fact]
    public void AssignRole_Should_AddRoleSuccessfully()
    {
        // Arrange
        var email = Email.Create("test@example.com");
        var user = User.Create("TestUser", email, "SecurePass123");
        var role = Role.CreateRole("Admin", "Admin role");
        
        // Act
        user.AssignRole(role);
        
        // Assert
        Assert.Contains(role, user.Roles);
    }
    
    [Fact]
    public void VerifyPassword_Should_ReturnTrueForValidPassword()
    {
        // Arrange
        var email = Email.Create("test@example.com");
        var password = "SecurePass123";
        var user = User.Create("TestUser", email, password);
        var mockPasswordHasher = new Mock<IPasswordHasher>();
        mockPasswordHasher.Setup(ph => ph.VerifyPassword(user.Password, password)).Returns(true);
        
        // Act
        var result = user.VerifyPassword(password, mockPasswordHasher.Object);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void CreatePatient_Should_AddPatientAndRaiseEvent()
    {
        // Arrange
        var email = Email.Create("test@example.com");
        var user = User.Create("TestUser", email, "SecurePass123");
        var patientEmail = Email.Create("patient@example.com");
        
        // Act
        var patient = user.CreatePatient("Patient Name", patientEmail, "123456789", "123 Street", new DateTime(1990, 1, 1));
        
        // Assert
        Assert.Contains(patient, user.Patients);
        Assert.Single(user.GetDomainEvents());
        Assert.IsType<CreatedPatientEvent>(user.GetDomainEvents().First());
    }
    
    [Fact]
    public void SetRefreshToken_Should_SetTokenSuccessfully()
    {
        // Arrange
        var email = Email.Create("test@example.com");
        var user = User.Create("TestUser", email, "SecurePass123");
        var token = "refresh-token";
        var expiryDate = DateTime.UtcNow.AddDays(7);
        
        // Act
        user.SetRefreshToken(token, expiryDate);
        
        // Assert
        Assert.NotNull(user.RefreshToken);
        Assert.Equal(token, user.RefreshToken.Token);
        Assert.Equal(expiryDate, user.RefreshToken.ExpiryDate);
    }
    
    [Fact]
    public void HasValidRefreshToken_Should_ReturnTrueForValidToken()
    {
        // Arrange
        var email = Email.Create("test@example.com");
        var user = User.Create("TestUser", email, "SecurePass123");
        var token = "refresh-token";
        var expiryDate = DateTime.UtcNow.AddDays(7);
        user.SetRefreshToken(token, expiryDate);
        
        // Act
        var result = user.HasValidRefreshToken(token);
        
        // Assert
        Assert.True(result);
    }
}