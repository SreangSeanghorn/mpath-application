using MPath.Domain.Entities;

namespace MPath.Tests.DomainTests.Roles;

public class RoleTests
{
    [Fact]
    public void CreateRole_Should_CreateRoleSuccessfully()
    {
        // Arrange
        var name = "Admin";
        var description = "Administrator role with full access";
        
        // Act
        var role = Role.CreateRole(name, description);
        
        // Assert
        Assert.NotNull(role);
        Assert.Equal(name, role.Name);
        Assert.Equal(description, role.Description);
    }
    
    [Fact]
    public void GetRole_Should_CreateRoleWithOnlyName()
    {
        // Arrange
        var name = "User";
        
        // Act
        var role = Role.GetRole(name);
        
        // Assert
        Assert.NotNull(role);
        Assert.Equal(name, role.Name);
        Assert.Null(role.Description);
    }
    
    [Fact]
    public void Equals_Should_ReturnTrueForSameRoleName()
    {
        // Arrange
        var role1 = Role.CreateRole("Admin", "Administrator role");
        var role2 = Role.CreateRole("Admin", "Different description");
        
        // Act
        var areEqual = role1.Equals(role2);
        
        // Assert
        Assert.True(areEqual);
    }
    
    [Fact]
    public void Equals_Should_ReturnFalseForDifferentRoleNames()
    {
        // Arrange
        var role1 = Role.CreateRole("Admin", "Administrator role");
        var role2 = Role.CreateRole("User", "User role");
        
        // Act
        var areEqual = role1.Equals(role2);
        
        // Assert
        Assert.False(areEqual);
    }
    
    [Fact]
    public void GetHashCode_Should_ReturnSameValueForSameRoleName()
    {
        // Arrange
        var role1 = Role.CreateRole("Admin", "Administrator role");
        var role2 = Role.CreateRole("Admin", "Administrator role");
        
        // Act
        var hash1 = role1.GetHashCode();
        var hash2 = role2.GetHashCode();
        
        // Assert
        Assert.Equal(hash1, hash2);
    }
}