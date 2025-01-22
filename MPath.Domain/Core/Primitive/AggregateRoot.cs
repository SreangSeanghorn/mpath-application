


using MPath.Domain.Core.Event;

namespace MPath.Domain.Core.Primitive
{
    public class AggregateRoot<TId> : Entity<TId>, IAggregateRoot
    {
        private readonly List<IDomainEvent> _events = new List<IDomainEvent>();
        public IReadOnlyList<IDomainEvent> Events => _events;

        protected void RaiseDomainEvents(IDomainEvent domainEvent)
        {
            _events.Add(domainEvent);
            Console.WriteLine($"Event Raised here:{domainEvent.GetType()} and here is the event detail: "+domainEvent);
        }
        public void ClearEvents()
        {
            _events.Clear();
        }

        public List<IDomainEvent> GetDomainEvents()
        {
            return _events;
        }

        
    }
}