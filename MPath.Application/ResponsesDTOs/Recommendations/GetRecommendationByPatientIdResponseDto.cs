namespace MPath.Application.ResponsesDTOs.Recommendations;

public record GetRecommendationByPatientIdResponseDto
{
    public Guid Id { get;}
    public string Title { get;}
    public string Content { get;}
    public bool IsCompleted { get;}
}