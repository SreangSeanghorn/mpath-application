using FluentValidation;
using Moq;
using MPath.Application.CommandHandlers;
using MPath.Application.Commands;
using MPath.Application.Exceptions.Patients;
using MPath.Application.Exceptions.Users;
using MPath.Domain;
using MPath.Domain.Entities;
using MPath.Domain.Repositories;
using MPath.Domain.ValueObjects;

namespace MPath.Tests.ApplicationTests.Pateints;

public class CreatePatientCommandHandlerTests
{
    private readonly Mock<IPatientRepository> _mockPatientRepository;
    private readonly Mock<IValidator<CreatePatientCommand>> _mockValidator;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly CreatePatientCommandHandler _handler;

    public CreatePatientCommandHandlerTests()
    {
        _mockPatientRepository = new Mock<IPatientRepository>();
        _mockValidator = new Mock<IValidator<CreatePatientCommand>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUserRepository = new Mock<IUserRepository>();
        
        _handler = new CreatePatientCommandHandler(
            _mockPatientRepository.Object, 
            _mockValidator.Object, 
            _mockUnitOfWork.Object, 
            _mockUserRepository.Object);
    }

    [Fact]
    public async Task Handle_Should_CreatePatientSuccessfully()
    {
        // Arrange
        var command = new CreatePatientCommand ("John Doe", "johndoe@example.com", "123456789", "123 Street", 
            new DateTime(1990, 1, 1), new Guid() );
        
        _mockValidator.Setup(v => v.Validate(command)).Returns(new FluentValidation.Results.ValidationResult());
        _mockPatientRepository.Setup(repo => repo.GetPatientByEmail(command.Email)).ReturnsAsync((Patient)null);
        _mockUserRepository.Setup(repo => repo.GetByIdAsync(command.UserId))
            .ReturnsAsync(User.Create("John Doe", Email.Create("johndoe@example.com"), "SecurePass123"));
         _mockUnitOfWork.Setup(u => u.SaveChangesAsync(CancellationToken.None)).ReturnsAsync(1);
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.Name, result.Name);
        Assert.Equal(command.Email, result.Email);
        Assert.Equal(command.Phone, result.Phone);
        Assert.Equal(command.Address, result.Address);
        Assert.Equal(command.DateOfBirth, result.DateOfBirth);
        _mockUserRepository.Verify(r=>r.GetByIdAsync(command.UserId), Times.Once);
        _mockPatientRepository.Verify(r=>r.AddAsync(It.IsAny<Patient>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenValidationFails()
    {
        // Arrange
        var command = new CreatePatientCommand ("", "invalid-email","", "", new DateTime(1990, 1, 1),new Guid() );
        var validationResult = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Email", "Invalid email format") });
        _mockValidator.Setup(v => v.Validate(command)).Returns(validationResult);
        
        // Act & Assert
        await Assert.ThrowsAsync<InvalidPatientCreatedException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenPatientAlreadyExists()
    {
        // Arrange
        var command = new CreatePatientCommand ("John Doe", "johndoe@example.com", "123456789", "123 Street", 
            new DateTime(1990, 1, 1), new Guid() );
        var existingPatient = Patient.Create("John Doe", Email.Create("johndoe@example.com"), "123456789", "123 Street", new DateTime(1990, 1, 1));
        _mockValidator.Setup(v => v.Validate(command)).Returns(new FluentValidation.Results.ValidationResult());
        _mockPatientRepository.Setup(repo => repo.GetPatientByEmail(command.Email)).ReturnsAsync(existingPatient);
        
        // Act & Assert
        await Assert.ThrowsAsync<PatientWithProvidedEmailAlreadyExistException>(() => _handler.Handle(command, CancellationToken.None));
        _mockPatientRepository.Verify(r=>r.GetPatientByEmail(command.Email), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenUserNotFound()
    {
        // Arrange
        var command = new CreatePatientCommand ("John Doe", "johndoe@example.com", "123456789", "123 Street", 
            new DateTime(1990, 1, 1), new Guid() );
        _mockValidator.Setup(v => v.Validate(command)).Returns(new FluentValidation.Results.ValidationResult());
        _mockPatientRepository.Setup(repo => repo.GetPatientByEmail(command.Email)).ReturnsAsync((Patient)null);
        _mockUserRepository.Setup(repo => repo.GetByIdAsync(command.UserId)).ReturnsAsync((User)null);
        
        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        _mockPatientRepository.Verify(r=>r.GetPatientByEmail(command.Email), Times.Once);
        _mockUserRepository.Verify(r=>r.GetByIdAsync(command.UserId), Times.Once);
    }
}