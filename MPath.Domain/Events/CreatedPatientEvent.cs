using MPath.Domain.EventDatas;
using MPath.SharedKernel.Event;

namespace MPath.Domain.Events;

public class CreatedPatientEvent : DomainEvent<CreatedPatientEventData>
{
    public CreatedPatientEvent(CreatedPatientEventData content) : base(content)
    {
    }
}