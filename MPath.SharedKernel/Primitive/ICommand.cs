using MediatR;
namespace MPath.SharedKernel.Primitive
{
    public interface ICommand : IRequest
    {
        
    }
    public interface ICommand<TResult> : IRequest<TResult>
    {
        
    } 

}