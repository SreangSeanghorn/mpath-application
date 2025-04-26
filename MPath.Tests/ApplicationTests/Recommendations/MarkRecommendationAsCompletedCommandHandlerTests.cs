using FluentValidation;
using Moq;
using MPath.Application.CommandHandlers;
using MPath.Application.Commands;
using MPath.Application.Exceptions.Recommendations;
using MPath.Application.Exceptions.Users;
using MPath.Domain;
using MPath.Domain.Entities;
using MPath.Domain.Repositories;
using MPath.Domain.ValueObjects;

namespace MPath.Tests.ApplicationTests.Recommendations;

public class MarkRecommendationAsCompletedCommandHandlerTests
{
    private readonly Mock<IPatientRepository> _mockPatientRepository;
    private readonly Mock<IRecommendationRepository> _mockRecommendationRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IValidator<MarkRecommendationAsCompletedCommand>> _mockValidator;
    private readonly MarkRecommendationAsCompletedCommandHandler _handler;

    public MarkRecommendationAsCompletedCommandHandlerTests()
    {
        _mockPatientRepository = new Mock<IPatientRepository>();
        _mockRecommendationRepository = new Mock<IRecommendationRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockValidator = new Mock<IValidator<MarkRecommendationAsCompletedCommand>>();
        
        _handler = new MarkRecommendationAsCompletedCommandHandler(
            _mockPatientRepository.Object,
            _mockRecommendationRepository.Object,
            _mockUserRepository.Object,
            _mockUnitOfWork.Object,
            _mockValidator.Object);
    }

    [Fact]
    public async Task Handle_Should_MarkRecommendationAsCompletedSuccessfully()
    {
        // Arrange
        var command = new MarkRecommendationAsCompletedCommand(Guid.NewGuid(), Guid.NewGuid());
        var user = User.Create("TestUser", Email.Create("test@gmail.com"), "SecurePass123");
        var recommendation =  Recommendation.Create("Exercise", "Do daily workouts", false, Guid.NewGuid());
        
        _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockUserRepository.Setup(repo => repo.GetByIdAsync(command.UserId)).ReturnsAsync(user);
        _mockRecommendationRepository.Setup(repo => repo.GetByIdAsync(command.RecommendationId)).ReturnsAsync(recommendation);
        _mockUserRepository.Setup(repo => repo.UpdateAsync(user)).Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.True(result);
        _mockUserRepository.Verify(repo => repo.UpdateAsync(user), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenValidationFails()
    {
        // Arrange
        var command = new MarkRecommendationAsCompletedCommand(Guid.NewGuid(), Guid.NewGuid());
        var validationResult = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("RecommendationId", "Invalid recommendation ID") });
        _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        
        // Act & Assert
        await Assert.ThrowsAsync<InvalidInputForMarkRecommendationAsCompletedException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenUserNotFound()
    {
        // Arrange
        var command = new MarkRecommendationAsCompletedCommand(Guid.NewGuid(), Guid.NewGuid());
        _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockUserRepository.Setup(repo => repo.GetByIdAsync(command.UserId)).ReturnsAsync((User)null);
        
        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }
}