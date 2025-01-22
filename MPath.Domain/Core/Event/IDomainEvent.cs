namespace MPath.Domain.Core.Event
{
    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }
        Guid EntityId { get; }
    }
}