using FluentValidation;
using Moq;
using MPath.Application.CommandHandlers;
using MPath.Application.Commands;
using MPath.Application.Exceptions.Patients;
using MPath.Application.Exceptions.Recommendations;
using MPath.Application.Exceptions.Users;
using MPath.Domain;
using MPath.Domain.Entities;
using MPath.Domain.Repositories;
using MPath.Domain.ValueObjects;

namespace MPath.Tests.ApplicationTests.Recommendations;

public class CreateRecommendationCommandHandlerTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IPatientRepository> _mockPatientRepository;
    private readonly Mock<IValidator<CreateRecommendationCommand>> _mockValidator;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly CreateRecommendationCommandHandler _handler;

    public CreateRecommendationCommandHandlerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockPatientRepository = new Mock<IPatientRepository>();
        _mockValidator = new Mock<IValidator<CreateRecommendationCommand>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        
        _handler = new CreateRecommendationCommandHandler(
            _mockUserRepository.Object,
            _mockPatientRepository.Object,
            _mockValidator.Object,
            _mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_Should_CreateRecommendationSuccessfully()
    {
        // Arrange
        var command = new CreateRecommendationCommand("Exercise", "Do daily workouts", Guid.NewGuid(), Guid.NewGuid());
        var user = User.Create("TestUser", Email.Create("test@example.com"), "SecurePass123");
        var patient = Patient.Create("John", Email.Create("mail@gmail.com"), "1234567890", "Some Address", new DateTime(1990,01,10));

        _mockValidator.Setup(v => v.Validate(command)).Returns(new FluentValidation.Results.ValidationResult());
        _mockUserRepository.Setup(repo => repo.GetUserWithPatient(command.UserId)).ReturnsAsync(user);
        _mockPatientRepository.Setup(repo => repo.GetByIdAsync(command.PatientId)).ReturnsAsync(patient);
        _mockUserRepository.Setup(repo => repo.UpdateAsync(user)).Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.Title, result.Title);
        Assert.Equal(command.Content, result.Content);
        Assert.Equal(command.PatientId, result.PatientId);
        _mockUserRepository.Verify(repo => repo.UpdateAsync(user), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenValidationFails()
    {
        // Arrange
        var command = new CreateRecommendationCommand("", "", Guid.NewGuid(), Guid.NewGuid());
        var validationResult = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Title", "Title is required") });
        _mockValidator.Setup(v => v.Validate(command)).Returns(validationResult);
        
        // Act & Assert
        await Assert.ThrowsAsync<InvalidInputForCreateRecommendationException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenUserNotFound()
    {
        // Arrange
        var command = new CreateRecommendationCommand("Exercise", "Do daily workouts", Guid.NewGuid(), Guid.NewGuid());
        _mockValidator.Setup(v => v.Validate(command)).Returns(new FluentValidation.Results.ValidationResult());
        _mockUserRepository.Setup(repo => repo.GetUserWithPatient(command.UserId)).ReturnsAsync((User)null);
        
        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenPatientNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new CreateRecommendationCommand("Exercise", "Do daily workouts", userId, Guid.NewGuid());

        var user = User.Create("TestUser", Email.Create("test@example.com"), "SecurePass123");

        _mockValidator.Setup(v => v.Validate(command)).Returns(new FluentValidation.Results.ValidationResult());
        _mockUserRepository.Setup(repo => repo.GetUserWithPatient(command.UserId)).ReturnsAsync(user);
        _mockPatientRepository.Setup(repo => repo.GetByIdAsync(command.PatientId)).ReturnsAsync((Patient)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<PatientWithProvidedIdNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Patient not found", exception.Message);  // Ensure the correct exception is thrown
    }
}