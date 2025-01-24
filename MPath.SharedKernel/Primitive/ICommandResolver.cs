
namespace MPath.SharedKernel.Primitive
{
    public interface ICommandResolver
    {
          Task<TResult> ResolveHandler<TCommand, TResult>(TCommand command)
           where TCommand : ICommand<TResult>;
    }
}