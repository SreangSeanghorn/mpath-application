using MPath.Domain.EventDatas;
using MPath.SharedKernel.Event;

namespace MPath.Domain.Events;

public class PatientAddRecommendationEvent : DomainEvent<PatientAddRecommendationEventData>
{
    public PatientAddRecommendationEvent(PatientAddRecommendationEventData content) : base(content)
    {
    }
}