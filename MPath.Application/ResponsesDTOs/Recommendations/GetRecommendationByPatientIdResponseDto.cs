namespace MPath.Application.ResponsesDTOs.Recommendations;

public record GetRecommendationByPatientIdResponseDto(

     Guid Id,
     string Title,
     string Content,
     bool IsCompleted
){}