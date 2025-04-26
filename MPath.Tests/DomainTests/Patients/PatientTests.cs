using MPath.Domain.Entities;
using MPath.Domain.ValueObjects;

namespace MPath.Tests.DomainTests.Patients;

public class PatientTests
{
    [Fact]
    public void Create_Should_CreatePatientSuccessfully()
    {
        // Arrange
        var name = "John Doe";
        var email = Email.Create("johndoe@example.com");
        var phoneNumber = "123456789";
        var address = "123 Street";
        var birthDate = new DateTime(1990, 1, 1);
        
        // Act
        var patient = Patient.Create(name, email, phoneNumber, address, birthDate);
        
        // Assert
        Assert.NotNull(patient);
        Assert.Equal(name, patient.Name);
        Assert.Equal(email, patient.Email);
        Assert.Equal(phoneNumber, patient.PhoneNumber);
        Assert.Equal(address, patient.Address);
        Assert.Equal(birthDate, patient.BirthDate);
    }
    
    [Fact]
    public void AddRecommendation_Should_AddRecommendationToPatient()
    {
        // Arrange
        var patient = Patient.Create("John Doe", Email.Create("johndoe@example.com"), "123456789", "123 Street", new DateTime(1990, 1, 1));
        var recommendation = Recommendation.Create("Exercise", "Do daily workouts", false, patient.Id);
        
        // Act
        patient.AddRecommendation(recommendation);
        
        // Assert
        Assert.Contains(recommendation, patient.Recommendations);
    }
}