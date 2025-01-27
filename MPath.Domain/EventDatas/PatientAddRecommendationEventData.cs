namespace MPath.Domain.EventDatas;

public record PatientAddRecommendationEventData(
    Guid PatientId,
    Guid RecommendationId,
    string Title,
    string Content
);