namespace MPath.SharedKernel.Event
{
    public interface IDomainEventHandler<TEvent> where TEvent : IDomainEvent
    {
        Task Handle(TEvent domainEvent,CancellationToken cancellationToken = default);
        
    }
}