namespace MPath.Tests.DomainTests.RefreshToken;

public class RefreshTokenTests
{
    [Fact]
    public void Create_Should_CreateRefreshTokenSuccessfully()
    {
        // Arrange
        var token = "sample-token";
        var expiryDate = DateTime.UtcNow.AddDays(7);
        
        // Act
        var refreshToken = Domain.Entities.RefreshToken.Create(token, expiryDate);
        
        // Assert
        Assert.NotNull(refreshToken);
        Assert.Equal(token, refreshToken.Token);
        Assert.Equal(expiryDate, refreshToken.ExpiryDate);
        Assert.False(refreshToken.IsRevoked);
    }
    
    [Fact]
    public void IsExpired_Should_ReturnFalse_IfTokenIsNotExpired()
    {
        // Arrange
        var refreshToken = Domain.Entities.RefreshToken.Create("sample-token", DateTime.UtcNow.AddDays(1));
        
        // Act
        var isExpired = refreshToken.IsExpired;
        
        // Assert
        Assert.False(isExpired);
    }
    
    [Fact]
    public void IsExpired_Should_ReturnTrue_IfTokenIsExpired()
    {
        // Arrange
        var refreshToken = Domain.Entities.RefreshToken.Create("sample-token", DateTime.UtcNow.AddDays(-1));
        
        // Act
        var isExpired = refreshToken.IsExpired;
        
        // Assert
        Assert.True(isExpired);
    }
    
    [Fact]
    public void Revoke_Should_SetIsRevokedToTrue()
    {
        // Arrange
        var refreshToken = Domain.Entities.RefreshToken.Create("sample-token", DateTime.UtcNow.AddDays(1));
        
        // Act
        refreshToken.Revoke();
        
        // Assert
        Assert.True(refreshToken.IsRevoked);
    }
    
    [Fact]
    public void Revoke_Should_ThrowException_IfAlreadyRevoked()
    {
        // Arrange
        var refreshToken = Domain.Entities.RefreshToken.Create("sample-token", DateTime.UtcNow.AddDays(1));
        refreshToken.Revoke();
        
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => refreshToken.Revoke());
    }
    
    [Fact]
    public void IsValid_Should_ReturnTrue_IfNotRevokedAndNotExpired()
    {
        // Arrange
        var refreshToken = Domain.Entities.RefreshToken.Create("sample-token", DateTime.UtcNow.AddDays(1));
        
        // Act
        var isValid = refreshToken.IsValid();
        
        // Assert
        Assert.True(isValid);
    }
    
    [Fact]
    public void IsValid_Should_ReturnFalse_IfRevoked()
    {
        // Arrange
        var refreshToken = Domain.Entities.RefreshToken.Create("sample-token", DateTime.UtcNow.AddDays(1));
        refreshToken.Revoke();
        
        // Act
        var isValid = refreshToken.IsValid();
        
        // Assert
        Assert.False(isValid);
    }
    
    [Fact]
    public void IsValid_Should_ReturnFalse_IfExpired()
    {
        // Arrange
        var refreshToken = Domain.Entities.RefreshToken.Create("sample-token", DateTime.UtcNow.AddDays(-1));
        
        // Act
        var isValid = refreshToken.IsValid();
        
        // Assert
        Assert.False(isValid);
    }
}