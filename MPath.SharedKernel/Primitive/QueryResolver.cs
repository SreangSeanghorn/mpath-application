using Microsoft.Extensions.DependencyInjection;

namespace MPath.SharedKernel.Primitive
{
    public class QueryResolver : IQueryResolver
    {
        private readonly IServiceProvider _serviceProvider;
        public QueryResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResult> ResolveHandler<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
        {
            var handler = _serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
            if (handler == null)
            {
                throw new InvalidOperationException($"Handler for {typeof(TQuery).Name} not found");
            }
            return await handler.Handle(query);
        }
    }
}

