namespace MPath.SharedKernel.Event
{
    public abstract class DomainEvent<T> : IDomainEvent
    {
        public DateTime OccurredOn { get; private set; }
        public T Content { get; private set; }
        protected DomainEvent(T content)
        {
            OccurredOn = DateTime.Now;
            Content = content;
        }
    }
}