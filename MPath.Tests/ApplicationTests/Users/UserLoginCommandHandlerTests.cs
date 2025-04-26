

using FluentValidation;
using Moq;
using MPath.Application.CommandHandlers;
using MPath.Application.Commands;
using MPath.Application.Exceptions.UserLogin;
using MPath.Domain.Core.Interfaces;
using MPath.Domain.Entities;
using MPath.Domain.Repositories;
using MPath.Domain.ValueObjects;
using MPath.Infrastructure.Authentication.JwtRefreshTokenGenerator;
using MPath.Infrastructure.Authentication.JwtTokenGenerator;

namespace MPath.Tests.ApplicationTests.Users;

public class UserLoginCommandHandlerTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IRoleRepository> _mockRoleRepository;
    private readonly Mock<IValidator<UserLoginCommand>> _mockValidator;
    private readonly Mock<IJwtTokenGenerator> _mockJwtTokenGenerator;
    private readonly Mock<IJwtRefreshTokenGenerator> _mockJwtRefreshTokenGenerator;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    private readonly UserLoginCommandHandler _handler;

    public UserLoginCommandHandlerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockRoleRepository = new Mock<IRoleRepository>();
        _mockValidator = new Mock<IValidator<UserLoginCommand>>();
        _mockJwtTokenGenerator = new Mock<IJwtTokenGenerator>();
        _mockJwtRefreshTokenGenerator = new Mock<IJwtRefreshTokenGenerator>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        
        _handler = new UserLoginCommandHandler(
            _mockUserRepository.Object,
            _mockRoleRepository.Object,
            _mockValidator.Object,
            _mockJwtTokenGenerator.Object,
            _mockJwtRefreshTokenGenerator.Object,
            _mockPasswordHasher.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnToken_WhenCredentialsAreValid()
    {
        // Arrange
        var command = new UserLoginCommand("test@example.com", "SecurePass123");
        var user = User.Create("TestUser", Email.Create(command.Email), "HashedPassword");
        var roles = new List<Role> { Role.CreateRole("Admin","Admin") };
        user.Roles = roles;
        
        _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockUserRepository.Setup(repo => repo.GetByEmail(command.Email)).ReturnsAsync(user);
        _mockPasswordHasher.Setup(ph => ph.VerifyPassword(user.Password, command.Password)).Returns(true);
        _mockJwtTokenGenerator.Setup(jt => jt.GenerateToken(user.Email.Value, It.IsAny<List<string>>())).Returns("jwt-token");
        _mockJwtRefreshTokenGenerator.Setup(jr => jr.GenerateRefreshToken()).Returns("refresh-token");
        _mockJwtRefreshTokenGenerator.Setup(jr => jr.GetExpiryDate()).Returns(DateTime.UtcNow.AddDays(7));
        _mockUserRepository.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("jwt-token", result.AccessToken);
        Assert.Equal("refresh-token", result.RefreshToken);
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenValidationFails()
    {
        // Arrange
        var command = new UserLoginCommand("", "");
        var validationResult = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Email", "Email is required") });
        _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        
        // Act & Assert
        await Assert.ThrowsAsync<InvalidUserLoginException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenUserNotFound()
    {
        // Arrange
        var command = new UserLoginCommand("test@example.com", "SecurePass123");
        _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockUserRepository.Setup(repo => repo.GetByEmail(command.Email)).ReturnsAsync((User)null);
        
        // Act & Assert
        await Assert.ThrowsAsync<UserWithProvidedEmailNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenPasswordIsIncorrect()
    {
        // Arrange
        var command = new UserLoginCommand("test@example.com", "WrongPass");
        var user = User.Create("TestUser", Email.Create(command.Email), "HashedPassword");
        
        _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockUserRepository.Setup(repo => repo.GetByEmail(command.Email)).ReturnsAsync(user);
        _mockPasswordHasher.Setup(ph => ph.VerifyPassword(user.Password, command.Password)).Returns(false);
        
        // Act & Assert
        await Assert.ThrowsAsync<IncorrectUserCredentialException>(() => _handler.Handle(command, CancellationToken.None));
    }
}