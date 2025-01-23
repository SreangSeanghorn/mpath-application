namespace MPath.SharedKernel.Event
{
    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }
    }
}