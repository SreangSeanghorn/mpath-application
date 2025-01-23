

using MPath.Domain.EventDatas;
using MPath.SharedKernel.Event;

namespace MPath.Domain.Events

{
    public class UserRegisteredEvent : DomainEvent<UserRegisteredEventData>
    {
        public UserRegisteredEvent(UserRegisteredEventData content) : base(content)
        {
        }
    }
}
