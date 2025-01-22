using MPath.Domain.Core.Event;
using MPath.Domain.EventDatas;

namespace MPath.Domain.Events

{
    public class UserRegisteredEvent : DomainEvent<UserRegisteredEventData>
    {
        public UserRegisteredEvent(Guid entityId, UserRegisteredEventData content) : base(entityId, content)
        {
        }
    }
}
