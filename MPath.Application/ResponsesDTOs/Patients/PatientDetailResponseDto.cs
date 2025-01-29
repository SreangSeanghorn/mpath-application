using MPath.Application.ResponsesDTOs.Recommendations;

namespace MPath.Application.ResponsesDTOs.Patients;

public record PatientDetailResponseDto(Guid Id, string Name, string Email, string PhoneNumber, string Address, DateTime BirthDate, IEnumerable<GetRecommendationByPatientIdResponseDto> Recommendations);