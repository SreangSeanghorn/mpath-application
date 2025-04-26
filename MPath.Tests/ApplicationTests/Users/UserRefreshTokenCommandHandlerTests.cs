using FluentValidation;
using Moq;
using MPath.Application.CommandHandlers;
using MPath.Application.Commands;
using MPath.Application.Exceptions.UserRefreshedToken;
using MPath.Domain;
using MPath.Domain.Entities;
using MPath.Domain.Repositories;
using MPath.Domain.ValueObjects;
using MPath.Infrastructure.Authentication.JwtTokenGenerator;

namespace MPath.Tests.ApplicationTests.Users;

public class UserRefreshTokenCommandHandlerTests
{
    private readonly Mock<IValidator<UserRefreshTokenCommand>> _mockValidator;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IJwtTokenGenerator> _mockJwtTokenGenerator;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly UserRefreshTokenCommandHandler _handler;

    public UserRefreshTokenCommandHandlerTests()
    {
        _mockValidator = new Mock<IValidator<UserRefreshTokenCommand>>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockJwtTokenGenerator = new Mock<IJwtTokenGenerator>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        
        _handler = new UserRefreshTokenCommandHandler(
            _mockValidator.Object,
            _mockUserRepository.Object,
            _mockJwtTokenGenerator.Object,
            _mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnNewToken_WhenRefreshTokenIsValid()
    {
        // Arrange
        var command = new UserRefreshTokenCommand("valid-refresh-token");
        var user = User.Create("TestUser", Email.Create("test@example.com"), "SecurePass123");
        var roles = new List<Role> { Role.CreateRole("Admin","Admin") };
        user.Roles = roles;
        
        _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockUserRepository.Setup(repo => repo.GetUserByRefreshTokenAsync(command.RefreshToken)).ReturnsAsync(user);
        _mockJwtTokenGenerator.Setup(jt => jt.GenerateToken(user.UserName, It.IsAny<List<string>>())).Returns("new-jwt-token");
        _mockJwtTokenGenerator.Setup(jt => jt.GenerateRefreshToken()).Returns("new-refresh-token");
        _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("new-jwt-token", result.AccessToken);
        Assert.Equal("new-refresh-token", result.RefreshToken);
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenValidationFails()
    {
        // Arrange
        var command = new UserRefreshTokenCommand("");
        var validationResult = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("RefreshToken", "Refresh token is required") });
        _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        
        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenTokenIsInvalid()
    {
        // Arrange
        var command = new UserRefreshTokenCommand("invalid-refresh-token");
        _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockUserRepository.Setup(repo => repo.GetUserByRefreshTokenAsync(command.RefreshToken)).ReturnsAsync((User)null);
        
        // Act & Assert
        await Assert.ThrowsAsync<InvalidTokenException>(() => _handler.Handle(command, CancellationToken.None));
    }
}