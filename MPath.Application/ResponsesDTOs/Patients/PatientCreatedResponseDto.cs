namespace MPath.Application.ResponsesDTOs.Patients;

public record PatientCreatedResponseDto(
    string Name,
    string Email,
    string Phone,
    string Address,
    DateTime DateOfBirth);