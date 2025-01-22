using MPath.Domain.Core.Event;
using MPath.Domain.EventDatas;

namespace MPath.Domain.Events;

public class AssignedRoleEvent : DomainEvent<AssignRoleEventData>
{
    public AssignedRoleEvent(Guid entityId, AssignRoleEventData content) : base(entityId, content)
    {
    }
}