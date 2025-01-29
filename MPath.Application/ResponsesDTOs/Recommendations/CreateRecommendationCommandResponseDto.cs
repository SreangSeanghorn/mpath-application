namespace MPath.Application.ResponsesDTOs.Recommendations;

public record CreateRecommendationCommandResponseDto(
    string Title,
    string Content,
    Guid PatientId
    );