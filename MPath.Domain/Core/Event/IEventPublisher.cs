namespace MPath.Domain.Core.Event
{
    public interface IEventPublisher
    {
        Task Publish<T>(T @event) where T : IDomainEvent;
    }
}