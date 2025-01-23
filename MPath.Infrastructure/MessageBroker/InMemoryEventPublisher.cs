using Microsoft.Extensions.DependencyInjection;
using MPath.SharedKernel.Event;

namespace MPath.Infrastructure.MessageBroker;

public class InMemoryEventPublisher : IEventPublisher
{
    private readonly IServiceProvider _serviceProvider;
    
    public InMemoryEventPublisher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public async Task Publish<T>(T @event) where T : IDomainEvent
    {
        var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(@event.GetType());
        var handlers = _serviceProvider.GetServices(handlerType);

        foreach (var handler in handlers)
        {
            var method = handler.GetType().GetMethod("Handle");
            if (method != null)
            {
                await (Task)method.Invoke(handler, new object[] { @event, CancellationToken.None });
            }
        }
        
    }
}