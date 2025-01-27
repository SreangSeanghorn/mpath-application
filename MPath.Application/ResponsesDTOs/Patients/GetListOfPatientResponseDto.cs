namespace MPath.Application.ResponsesDTOs.Patients;

public record GetListOfPatientResponseDto(
    Guid Id,
    string Name,
    string Email,
    string PhoneNumber,
    string Address,
    DateTime DateOfBirth)
{
   
}