
using MPath.Domain.EventDatas;
using MPath.SharedKernel.Event;

namespace MPath.Domain.Events;

public class AssignedRoleEvent : DomainEvent<AssignRoleEventData>
{
    public AssignedRoleEvent(AssignRoleEventData content) : base( content)
    {
    }
}