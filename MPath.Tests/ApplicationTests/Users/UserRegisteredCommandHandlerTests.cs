using FluentValidation;
using Moq;
using MPath.Application.CommandHandlers;
using MPath.Application.Commands;
using MPath.Application.Exceptions.Roles;
using MPath.Application.Exceptions.UserRegistered;
using MPath.Domain;
using MPath.Domain.Core.Interfaces;
using MPath.Domain.Entities;
using MPath.Domain.Repositories;
using MPath.Domain.ValueObjects;

namespace MPath.Tests.ApplicationTests.Users;

public class UserRegisteredCommandHandlerTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    private readonly Mock<IValidator<UserRegisterCommand>> _mockValidator;
    private readonly Mock<IRoleRepository> _mockRoleRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly UserRegisteredCommandHandler _handler;

    public UserRegisteredCommandHandlerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        _mockValidator = new Mock<IValidator<UserRegisterCommand>>();
        _mockRoleRepository = new Mock<IRoleRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        
        _handler = new UserRegisteredCommandHandler(
            _mockUserRepository.Object,
            _mockPasswordHasher.Object,
            _mockValidator.Object,
            _mockRoleRepository.Object,
            _mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_Should_RegisterUserSuccessfully()
    {
        // Arrange
        var command = new UserRegisterCommand("TestUser", "test@example.com", "SecurePass123");
        var hashedPassword = "hashed-password";
        var role =  Role.CreateRole("Admin", "Admin role");
        
        _mockValidator.Setup(v => v.Validate(command)).Returns(new FluentValidation.Results.ValidationResult());
        _mockUserRepository.Setup(repo => repo.GetByEmail(command.Email)).ReturnsAsync((User)null);
        _mockPasswordHasher.Setup(ph => ph.HashPassword(command.Password)).Returns(hashedPassword);
        _mockRoleRepository.Setup(repo => repo.GetByIdAsync(command.RoleId)).ReturnsAsync(role);
        _mockUserRepository.Setup(repo => repo.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.UserName, result.UserName);
        Assert.Equal(command.Email, result.Email);
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenValidationFails()
    {
        // Arrange
        var command = new UserRegisterCommand("", "", "");
        var validationResult = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Email", "Email is required") });
        _mockValidator.Setup(v => v.Validate(command)).Returns(validationResult);
        
        // Act & Assert
        await Assert.ThrowsAsync<InvalidUserRegisterException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenUserAlreadyExists()
    {
        // Arrange
        var command = new UserRegisterCommand("TestUser", "test@example.com", "SecurePass123");
        var existingUser = User.Create("TestUser", Email.Create(command.Email), "SecurePass123");
        
        _mockValidator.Setup(v => v.Validate(command)).Returns(new FluentValidation.Results.ValidationResult());
        _mockUserRepository.Setup(repo => repo.GetByEmail(command.Email)).ReturnsAsync(existingUser);
        
        // Act & Assert
        await Assert.ThrowsAsync<UserAlreadyExistException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenRoleNotFound()
    {
        // Arrange
        var command = new UserRegisterCommand("TestUser", "test@example.com", "SecurePass123");
        
        _mockValidator.Setup(v => v.Validate(command)).Returns(new FluentValidation.Results.ValidationResult());
        _mockUserRepository.Setup(repo => repo.GetByEmail(command.Email)).ReturnsAsync((User)null);
        _mockRoleRepository.Setup(repo => repo.GetByIdAsync(command.RoleId)).ReturnsAsync((Role)null);
        
        // Act & Assert
        await Assert.ThrowsAsync<RoleNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }
}