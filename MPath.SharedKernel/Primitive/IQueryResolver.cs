

namespace MPath.SharedKernel.Primitive
{
    public interface IQueryResolver
    {
        Task<TResult> ResolveHandler<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;
    }
}

